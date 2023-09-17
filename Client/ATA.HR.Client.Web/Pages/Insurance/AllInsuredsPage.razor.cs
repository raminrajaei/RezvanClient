
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Insurance.Models;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using BlazorDownloadFile;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
// ReSharper disable MemberCanBePrivate.Global

namespace ATA.HR.Client.Web.Pages.Insurance;

[Authorize]
public partial class AllInsuredsPage : IDisposable
{
    // Consts
    private const int PageSize = 20;

    // Props
    public int TotalInsuredsCount { get; set; }
    private bool IsLoading { get; set; } = true;
    public List<string> AllUnits { get; set; } = new();
    public List<string> AllWorkLocations { get; set; } = new();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public GetAllInsuredsQuery AllInsuredsQuery { get; set; } = new();
    public AllInsuredsFilterArgs AllInsuredsFilterArgs { get; set; } = new()
    {
        OnlyCancelRequestsSelectedValue = "all"
    };

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> UnitsSource { get; set; } = new();
    private List<SelectListItem> WorkLocationsSource { get; set; } = new();
    private List<SelectListItem> InsuranceStatusSource { get; set; } = new();
    private List<SelectListItem> LifeInsuranceStatusSource { get; set; } = new();
    private List<SelectListItem> InsuredTypeSource { get; set; } = new()
    {
        new SelectListItem("فقط اصلی", "onlymain"),
        new SelectListItem("فقط تحت تکفل", "onlydependent")
    };
    private List<SelectListItem> CancellingTypeSource { get; set; } = new()
    {
        new SelectListItem("همه", "all"),
        new SelectListItem("درخواست‌های لغو", "cancel")
    };
    public bool SendSmsToActiveInsureds { get; set; }

    public bool IsVisibleTerminateInsuredConfirmDialog { get; set; }
    public bool IsVisibleReviveInsuredConfirmDialog { get; set; }

    public string TerminatingInsuredFullName { get; set; }
    public string RevivingInsuredFullName { get; set; }

    public int TerminatingInsuredId { get; set; }
    public int RevivingInsuredId { get; set; }

    // Index
    private TelerikGrid<InsuredDto> InsuredsGridRef { get; set; }

    // Inject
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
            SearchSubscriber();

            // Units
            //AllUnits = await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            //UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();

            // WorkLocations
            //AllWorkLocations = await HttpClient.User().GetAllWorkLocations(cancellationToken: cancellationToken);

            //WorkLocationsSource = AllWorkLocations.Select(wl => new SelectListItem(wl, wl)).ToList();

            // Insurance Status
            InsuranceStatusSource = await CoreAPIs.Insurance_GetAllInsuranceStatusTypes();
            LifeInsuranceStatusSource = await CoreAPIs.Insurance_GetAllLifeInsuranceStatusTypes();
            InsuranceStatusSource = InsuranceStatusSource.OrderBy(i => i.ValueInt == 4).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<InsuredDto> obj)
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

        return InsuredsGridRef.SetState(InsuredsGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                InsuredsGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var top = (args.Request.Page - 1) * args.Request.PageSize;

            var odataParameters = new ODataParameters(true, args.Request.PageSize, top);

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            AllInsuredsQuery = GetAllInsuredsQueryFromAllInsuredsFilterArgs();

            // With Refit Directly
            // ODataResponse<InsuredDto> odataResult = await CoreAPIs.Insurance_GetAllInsureds(AllInsuredsQuery, odataParameters, cancellationToken);

            // With HttpClient Of Refit
            var httpResponseMessage = await CoreAPIs.Client.PostAsJsonAsync<GetAllInsuredsQuery>($"/insurance/odata/AllInsureds?{oDataQuery}&$orderby=UserIdMainInsured", AllInsuredsQuery, cancellationToken);
            ODataResponse<InsuredDto> odataResult = await httpResponseMessage.Content.ReadFromJsonAsync<ODataResponse<InsuredDto>>(cancellationToken: cancellationToken);

            args.Data = odataResult.Value;

            args.Total = TotalInsuredsCount = odataResult.TotalCount;
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    private GetAllInsuredsQuery GetAllInsuredsQueryFromAllInsuredsFilterArgs()
    {
        return new GetAllInsuredsQuery
        {
            SearchTerm = AllInsuredsFilterArgs.SearchTerm,
            OnlyMainInsureds = AllInsuredsFilterArgs.OnlyMainInsuredsOrDependentsSelectedValue == "onlymain",
            OnlyDependents = AllInsuredsFilterArgs.OnlyMainInsuredsOrDependentsSelectedValue == "onlydependent",
            OnlyCancelRequests = AllInsuredsFilterArgs.OnlyCancelRequestsSelectedValue == "cancel",
            InsuranceStatus = string.IsNullOrWhiteSpace(AllInsuredsFilterArgs.InsuranceStatusSelectedValue)
                ? null
                : AllInsuredsFilterArgs.InsuranceStatusSelectedValue.ToInt(),
            LifeInsuranceStatus = string.IsNullOrWhiteSpace(AllInsuredsFilterArgs.LifeInsuranceStatusSelectedValue)
                ? null
                : AllInsuredsFilterArgs.LifeInsuranceStatusSelectedValue.ToInt(),
            FromEndedAtJalali = AllInsuredsFilterArgs.FromEndedAtJalali,
            ToEndedAtJalali = AllInsuredsFilterArgs.ToEndedAtJalali
        };
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

        if (AllInsuredsFilterArgs.SearchTerm != searchTerm)
            AllInsuredsFilterArgs.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AllInsuredsFilterArgs.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var insured = (InsuredDto)obj.Item;

        if (insured.IsMainInsurer && insured.IsActive)
            obj.Class = "main-insurer-row";

        else if (insured.IsMainInsurer && insured.IsActive is false)
            obj.Class = "main-insurer-cancelled-row";

        else if (insured.IsMainInsurer is false && insured.IsActive)
            obj.Class = "dependent-insurer-active-row";

        else if (insured.IsMainInsurer is false && insured.IsActive is false)
            obj.Class = "dependent-insurer-cancelled-row";
    }

    private void OpenUserDocumentsPage(int userId)
    {
        NavigationManager.NavigateTo(PageUrls.PersonnelDocumentsPage(userId));
    }

    private void OnUserImageLoadFailed(UserToManageDocumentDto user)
    {
        user.PictureURLToDisplay = "/images/layout/user.png";
    }

    private async Task<DownloadFileResult> ExportInsuredsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            AllInsuredsQuery = GetAllInsuredsQueryFromAllInsuredsFilterArgs();

            var reportData = await CoreAPIs.Insurance_GetAllInsureds(AllInsuredsQuery, new ODataParameters(), CancellationToken.None);

            if (reportData is null || reportData.TotalCount == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                var insuredDateToExport = reportData.Value;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"لیست بیمه شدگان")
                    .CreateGridLayoutExcel()
                    .WithOneSheetUsingModelBinding(insuredDateToExport)
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

    private void OpenSendSmsWindow()
    {
        throw new NotImplementedException();
    }

    private async Task TerminateInsured(int terminatingInsuredId)
    {
        try
        {
            await CoreAPIs.Insurance_CancelInsurance(terminatingInsuredId);

            NotificationService.Toast(NotificationType.Success, $"لغو عضویت {TerminatingInsuredFullName} با موفقیت انجام شد");
        }
        catch
        {
            // ignored
        }
        finally
        {
            await RebindGrid(false);

            IsVisibleTerminateInsuredConfirmDialog = false;

            StateHasChanged();
        }
    }

    private void ShowTerminateInsuredConfirmDialog(int insuredId, string insuredFullName)
    {
        TerminatingInsuredId = insuredId;

        TerminatingInsuredFullName = insuredFullName;

        IsVisibleTerminateInsuredConfirmDialog = true;
    }

    private void ShowReviveInsuredConfirmDialog(int insuredId, string insuredFullName)
    {
        RevivingInsuredId = insuredId;

        RevivingInsuredFullName = insuredFullName;

        IsVisibleReviveInsuredConfirmDialog = true;
    }

    private async Task ReviveInsured(int revivingInsuredId)
    {
        try
        {
            await CoreAPIs.Insurance_ReviveInsuredToActiveState(revivingInsuredId);

            NotificationService.Toast(NotificationType.Success, $"برگشت عضویت {RevivingInsuredFullName} با موفقیت انجام شد");
        }
        catch
        {
            // ignored
        }
        finally
        {
            await RebindGrid(false);

            IsVisibleReviveInsuredConfirmDialog = false;

            StateHasChanged();
        }
    }
}