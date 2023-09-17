using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Enums.Contract;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Helper.Extensions;
using ATABit.Helper.Utils;
using ATABit.Shared;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using BootstrapBlazor.Components;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Contract;

[Authorize]
public partial class AllContractsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    private string? LoadingText { get; set; }
    public List<string> AllUnits { get; set; } = new();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public int TotalCount { get; set; }
    public bool IsAdmin { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public AllContractsFilterArgs AllContractsFilter { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> UnitsSource { get; set; } = new();
    private List<SelectListItem> WorkLocationsSource { get; set; } = new();
    private List<SelectListItem> FlowStatesSource => EnumMapping.ToSelectListItems<FlowStatus>();
    private List<SelectListItem> ContractProgressSource => EnumMapping.ToSelectListItems<ContractProgressType>();
    public bool IsVisibleSendingSmsModal { get; set; }
    public string? SmsContentToSend { get; set; }
    public bool ShowSendWarningSmsToPersonnelOverSignButton => AllContractsFilter.ContractProgressType == ContractProgressType.WaitingForEmployeeSignature.ToString("D");

    // Index
    private TelerikGrid<ContractReadDto> ContractsGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }


    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            IsAdmin = await AuthenticationStateTask.IsAdminUser();

            SearchSubscriber();

            // Units
            AllUnits = await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();

            // WorkLocations
            var workLocations = await HttpClient.User().GetAllWorkLocations(cancellationToken: cancellationToken);

            WorkLocationsSource = workLocations.Select(wl => new SelectListItem(wl, wl)).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<ContractReadDto> obj)
    {

    }

    // Methods

    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        return ContractsGridRef.SetState(ContractsGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                ContractsGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            args.Data = await HttpClient.Contract().GetAllContracts(AllContractsFilter, context, cancellationToken);

            args.Total = TotalCount = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public Task ApplyFilters => RebindGrid(true);

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private void SearchSubscriber()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(t => t.IsNotNullOrEmpty() && (t!.Length > 2 || t.IsInt()) || string.IsNullOrEmpty(t))
            .Subscribe(async _ =>
            {
                try
                {
                    IsLoading = true;

                    _searchCancellationTokenSource.Cancel();

                    _searchCancellationTokenSource.Dispose();

                    _searchCancellationTokenSource = new CancellationTokenSource();

                    await RebindGrid(true, true);
                }
                catch (Exception exp)
                {
                    ExceptionHandler.OnExceptionReceived(exp);
                }
                finally
                {
                    StateHasChanged();
                }
            });
    }

    public void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (AllContractsFilter.SearchTerm != searchTerm)
            AllContractsFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AllContractsFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var user = (ContractReadDto)obj.Item;

        //if (((UserDto)user.OrderStatus).IsDelete())
        //    obj.Class = "deleted-order";
    }

    private void OpenContractFlowFormsPage(int contractId)
    {
        NavigationManager.NavigateTo(PageUrls.ContractFlowFormsPage(contractId));
    }

    private async Task<DownloadFileResult> ExportContractsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            LoadingText = "در حال ساخت گزارش اکسل";

            StateHasChanged();

            var reportData = await HttpClient.Contract().GetAllContracts(AllContractsFilter);

            if (reportData is null || reportData.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                //reportData = reportData.OrderBy(wh => wh.UserEmployeePersonnelCode!.Value).ToList();

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"قراردادهای پرسنل")
                    .CreateGridLayoutExcel()
                    .WithOneSheetUsingModelBinding(reportData)
                    .Build();

                downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
            }
        }
        catch (Exception e)
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            LoadingText = "";

            StateHasChanged();
        }

        return downloadFileResult;
    }

    private void ShowSendingSmsModal()
    {
        SmsContentToSend = $@"سلام
با توجه به عدم امضای قرارداد استخدامی خود لطفا به قید فوریت نسبت به امضای آن اقدام نمایید.";
        IsVisibleSendingSmsModal = true;
    }

    private async Task SendSMS()
    {
        if (string.IsNullOrWhiteSpace(SmsContentToSend))
        {
            NotificationService.Toast(NotificationType.Error, "خطا در عملیات، متن پیامک نمی‌تواند خالی باشد");

            return;
        }

        IsLoading = true;

        try
        {
            var msg = "";

            await HttpClient.Contract().SendContractsRelatedSmses(new ContractsSmsArgs
            {
                ContractsFilterArgs = AllContractsFilter,
                SmsContent = SmsContentToSend
            });

            IsVisibleSendingSmsModal = false;

            msg = "همه‌ی پیامک ارسال شدند";

            NotificationService.Toast(NotificationType.Success, msg);
        }
        catch (Exception e)
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}