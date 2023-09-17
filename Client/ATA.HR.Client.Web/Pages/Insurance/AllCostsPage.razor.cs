using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.HRInsuranceModels;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ATA.HR.Client.Web.APIs.Enums;
using ATABit.Helper.Utils;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;

namespace ATA.HR.Client.Web.Pages.Insurance;

[Authorize(Claims.Insurance_Calculations)]
public partial class AllCostsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    public int TotalCostsCounts { get; set; }
    private bool IsLoading { get; set; } = true;
    public List<CostDto> AllCosts { get; set; } = new();
    public List<CostDto> CalculatingCost { get; set; } = new();
    public long TotalCostsAmount { get; set; }
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public GetCostsQuery AllCostsQuery { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<PolicyObligationDto> ActivePolicyObligations { get; set; } = new();
    private List<SelectListItem> ActivePolicyObligationsSource { get; set; } = new();
    private List<SelectListItem> WorkLocationsSource { get; set; } = new();
    public bool IsVisibleRegisterCostConfirmDialog { get; set; }
    public int RegisteringCostId { get; set; }
    public List<BankPaymentDto> AllBankPayments { get; set; } = new();

    private List<SelectListItem> CostCurrentStatusFilterSource { get; set; } = EnumMapping.ToSelectListItems<CostCurrentStatusFilter>(); 
    
    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<CostDto> SelectedItems { get; set; } = Enumerable.Empty<CostDto>();
    public bool IsVisibleMultipleActionButton => TotalCostsCounts > 0;
    private string? SearchTermText { get; set; }
    private bool IsFirstLoad { get; set; }

    // Index
    private TelerikGrid<CostDto> CostsGridRef { get; set; }

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ICoreAPIs CoreAPIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }


    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            AllCostsQuery.SearchTerm = AppData.SearchTermAllCostsQueryAllCostsPage;

            Console.WriteLine($"SearchTerm: {AppData.SearchTermAllCostsQueryAllCostsPage}");

            AppData.CostsToPrint = new();

            ActivePolicyObligations = await CoreAPIs.Insurance_GetActivePolicyObligations();
            ActivePolicyObligationsSource = ActivePolicyObligations
                .Select(o => new SelectListItem($"{o.ObligationCode} – {o.ObligationTitle}", o.Id.ToString()))
                .ToList();

            var workLocations = await HttpClient.User().GetAllWorkLocations(cancellationToken: cancellationToken);

            WorkLocationsSource = workLocations.Select(x => new SelectListItem(x, x)).ToList();

            SearchTermText = AppData.SearchTermAllCostsQueryAllCostsPage;

            AllBankPayments = await CoreAPIs.Insurance_GetAllBankPayments();

            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<CostDto> obj)
    {
    }

    // Methods

    public string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{HttpClient.BaseAddress}{relativeUrl}";
    }

    public string ToDocFullURL(string filePath)
    {
        return $"{AppConstants.AppCDNBaseURL}{filePath}";
    }

    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private async Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        SelectedItems = Enumerable.Empty<CostDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        await CostsGridRef.SetState(CostsGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                CostsGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            if (AppData.SearchTermAllCostsQueryAllCostsPage.IsNotNullOrEmpty() && IsFirstLoad)
            {
                AllCostsQuery.SearchTerm = AppData.SearchTermAllCostsQueryAllCostsPage;

                IsFirstLoad = false;
            }

            TotalCostsAmount = await CoreAPIs.Insurance_GetAllCostsSum(AllCostsQuery);

            // With HttpClient Of Refit
            var httpResponseMessage = await CoreAPIs.Client.PostAsJsonAsync<GetCostsQuery>($"/insurance/odata/AllCosts?$expand=CostAttachments&$orderby=Id desc&{oDataQuery}", AllCostsQuery, cancellationToken);
            ODataResponse<CostDto> odataResult = await httpResponseMessage.Content.ReadFromJsonAsync<ODataResponse<CostDto>>(cancellationToken: cancellationToken);

            args.Data = odataResult.Value;

            args.Total = TotalCostsCounts = odataResult.TotalCount;
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    private async Task ApplyFilters()
    {
        await RebindGrid(true);
    }

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
                    //ExceptionHandler.OnExceptionReceived(exp);
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

        AppData.SearchTermAllCostsQueryAllCostsPage = SearchTermText = searchTerm;

        if (AllCostsQuery.SearchTerm != searchTerm)
            AllCostsQuery.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AllCostsQuery.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        //var user = (InsuredDto)obj.Item;

        //if (user.Dismissed)
        //    obj.Class = "contract-ending-alert";
    }

    private async Task PrintFilteredCosts()
    {
        List<CostDto> costsToPrint;

        if (IsMultiActionOnCustomSelection is false)
        {
            var httpResponseMessage = await CoreAPIs.Client.PostAsJsonAsync<GetCostsQuery>($"/insurance/odata/AllCosts?$count=true", AllCostsQuery);
            ODataResponse<CostDto> odataResult = await httpResponseMessage.Content.ReadFromJsonAsync<ODataResponse<CostDto>>();

            costsToPrint = odataResult.Value;
        }
        else
        {
            costsToPrint = SelectedItems.ToList();
        }

        if (costsToPrint.Count == 0)
        {
            NotificationService.Toast(NotificationType.Error, "هیچ رکورد هزینه‌ای برای پرینت انتخاب نشده است یا وجود ندارد");

            return;
        }

        // Check only one user is selected
        if (costsToPrint.Select(c => c.UserIdRegistrar).Distinct().Count() > 1)
        {
            NotificationService.Toast(NotificationType.Error, "گزارش پرینت فقط برای هزینه‌های یک کاربر قابل انجام می‌باشد");

            return;
        }

        AppData.CostsToPrint = costsToPrint;

        var mainInsuredUser = await HttpClient.User().GetUserById(costsToPrint.First().UserIdRegistrar);

        var hasSignature = await HttpClient.User().HasUserActiveSignature(mainInsuredUser.UserId);

        if (hasSignature is false)
        {
            NotificationService.Toast(NotificationType.Error, $"امضای پرسنل {mainInsuredUser.FullName} تعریف نشده است و نمی‌توانید پرینت هزینه‌های وی را دریافت نمایید");

            return;
        }

        AppData.CostsPrintInfo = (mainInsuredUser.FullName, mainInsuredUser.PersonnelCode.ToString(), mainInsuredUser.SignatureUrl);

        NavigationManager.NavigateTo(PageUrls.AllCostsPrintPage);
    }

    private void ChangeToAddMode()
    {
        //SetCostCommand = new SetCostCommand();

        //PageOperationType = OperationType.Add;
    }

    private async Task OnProcessCostSubmit()
    {
        if (ValidateCostOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            if (PageOperationType is OperationType.Add)
            {
                //var newCost = await CoreAPIs.Insurance_SetCost(SetCostCommand);

                //NewAddedCosts.Add(newCost);

                //NotificationService.Toast(NotificationType.Success, "هزینه با موفقیت ثبت شد");
            }
            else
            {
                //await HttpClient.PersonnelDocument().EditDocument(new PersonnelDocumentEditDto(PersonnelDocument.Id!.Value!, PersonnelDocument.Description));

                //NotificationService.Toast(NotificationType.Success, "سند با موفقیت ویرایش شد");
            }

            await RebindGrid(false);

            ChangeToFilterMode();
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private bool ValidateCostOnSubmit()
    {
        //if (SetCostCommand.CostAttachments.Count == 0)
        //{
        //    NotificationService.Toast(NotificationType.Error, "هیچ پیوستی بارگزاری نشده است. حداقل یک پیوست بارگزاری نمایید.");

        //    return false;
        //}

        return true;
    }

    private async Task<DownloadFileResult> ExportCostsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            // With HttpClient Of Refit
            var httpResponseMessage = await CoreAPIs.Client.PostAsJsonAsync<GetCostsQuery>($"/insurance/odata/AllCosts?count=true", AllCostsQuery);
            ODataResponse<CostDto> odataResult = await httpResponseMessage.Content.ReadFromJsonAsync<ODataResponse<CostDto>>();

            var reportData = odataResult.Value;

            if (reportData is null || reportData.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"لیست هزینه‌ها")
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

            StateHasChanged();
        }

        return downloadFileResult;
    }

    private void OpenCostFlowFormsPage(int costId)
    {
        NavigationManager.NavigateTo(PageUrls.InsuranceCostFlowFormsPage(costId, true));
    }
}