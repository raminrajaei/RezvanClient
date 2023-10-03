using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.APIs.Models.Response;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
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
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;

namespace ATA.HR.Client.Web.Pages.AdultStudent;

[Authorize]
public partial class AdultClassPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    private int TotalAdultsCount { get; set; }
    private bool IsLoading { get; set; } = true;
    private List<AdultClassOutputDto> AllAdults { get; set; } = new();
    public List<AdultClassOutputDto> NewAddedAdult { get; set; } = new(); //Only if we want to show the recently added students
    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private bool ResetPagination { get; set; }
    private bool IsRebindCalledBySearchSubscriber { get; set; }
    public List<SelectListItem> YearsSource { get; set; } = new();

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    private AdultClassInputDto AdultsFilter { get; set; } = new();
    private Subject<string> SearchSubject { get; } = new();

    #endregion

    public AdultUpsertDto AdultUpsertDto { get; set; } = new();
    // public CostAttachmentCommand CostAttachment1 { get; set; } = new();
    private bool IsVisibleDeleteAdultConfirmDialog { get; set; }
    private long DeletingAdultClassId { get; set; }
    private bool SavingFirstAdult { get; set; } = true;

    // Upload Props
    private bool IsUploading { get; set; }
    private string? UploadingMsg { get; set; }

    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<AdultClassOutputDto> SelectedItems { get; set; } = Enumerable.Empty<AdultClassOutputDto>();
    private bool IsVisibleMultipleActionButton => TotalAdultsCount > 0;

    // Index
    private TelerikGrid<AdultClassOutputDto> AdultsGridRef { get; set; }

    // Parameter
    [Parameter] public int? FromAddNewAdultToClassPage { get; set; }
    private bool IsNavigatedFromAddNewAdultToClassPage => FromAddNewAdultToClassPage is 1;

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IRezvanAPIs APIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }


    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            var adults = await APIs.GetAdultsClasses(AdultsFilter);

            YearsSource.GetYears();

            if (adults.IsSuccess)
            {
                AllAdults = adults.Data.Data;
            } 

            SearchSubscriber();
        }
        finally
        {
            IsLoading = true; //Will false in OnParamSet event
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    protected override Task OnParametersSetAsync()
    {
        if (IsNavigatedFromAddNewAdultToClassPage)
            PageOperationType = OperationType.Add;

        StateHasChanged();

        IsLoading = false;

        return base.OnParametersSetAsync();
    }


    private void OnStateInitHandler(GridStateEventArgs<AdultClassOutputDto> obj)
    {

    }

    // Methods

    private string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{HttpClient.BaseAddress}{relativeUrl}";
    }

    private string ToDocFullURL(string filePath)
    {
        return $"{AppConstants.AppCDNBaseURL}{filePath}";
    }

    private async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private async Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        SelectedItems = Enumerable.Empty<AdultClassOutputDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

        await AdultsGridRef.SetState(AdultsGridRef.GetState());
    }

    private async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                AdultsGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            AdultsFilter.Page = args.Request.Page;
            AdultsFilter.PageSize = PageSize;

            var adultsClasses = await APIs.GetAdultsClasses(AdultsFilter);

            if (adultsClasses.IsSuccess)
            {
                AllAdults = adultsClasses.Data.Data;

                //args.Data = IsNavigatedFromAddNewAdultToClassPage ? NewAddedCosts : AllMyCosts;

                args.Data = AllAdults;

                args.Total = TotalAdultsCount = adultsClasses.Data.TotalCount;
                //IsNavigatedFromAddNewAdultToClassPage
                //? NewAddedCosts.Count
                //: AllMyCosts.Count;  //(int)(context.TotalCount ?? 0);
            }
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

    private void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (AdultsFilter.SearchTerm != searchTerm)
            AdultsFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AdultsFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var cost = (AdultClassOutputDto)obj.Item;

        //if (cost.FlowCurrentStateTag == InsuranceStateTag.CostEdit.ToString())
        //    obj.Class = "cost-flaw-row";
    }

    private void OpenAddNewAdultClassPage()
    {
        NavigationManager.NavigateTo(PageUrls.AddAdultClassFormPage());
    }

    private async Task<DownloadFileResult> ExportCostsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var adultsExcelFilter = AdultsFilter.SerializeToJson()!.DeserializeToModel<AdultClassInputDto>();

            adultsExcelFilter.PageSize = Int32.MaxValue;
            adultsExcelFilter.Page = 1;

            var apiResult = await APIs.GetAdultsClasses(adultsExcelFilter);

            if (apiResult.IsSuccess is false || apiResult.Data.TotalCount == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                var reportData = apiResult.Data.Data;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName("لیست کلاس های بزرگسالان")
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

    private void OpenEditAdultClassPage(int adultId)
    {
        NavigationManager.NavigateTo(PageUrls.EditAdultClassFormPage(adultId));
    }

    private void ShowDeleteAdultClassConfirmDialog(long id)
    {
        DeletingAdultClassId = id;

        IsVisibleDeleteAdultConfirmDialog = true;
    }

    private async Task DeleteAdultClass(long deletingAdultId)
    {
        try
        {
            await APIs.DeleteAdultClass(deletingAdultId);

            NotificationService.Toast(NotificationType.Success, "حذف کلاس کودک با موفقیت انجام شد");
        }
        catch
        {
            // ignored
        }
        finally
        {
            await RebindGrid(false);

            StateHasChanged();
        }
    }

    async Task OnChange(object? value)
    {
        if (!string.IsNullOrEmpty(value?.ToString()))
        {
            AdultsFilter.Year = value.ToString();
            await RebindGrid(true, true);
        }
    }
}