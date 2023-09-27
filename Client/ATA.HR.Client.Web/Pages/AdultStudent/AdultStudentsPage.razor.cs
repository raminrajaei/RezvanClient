using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using ATABit.Helper.Extensions;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ATA.HR.Client.Web.APIs.Models.Response;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;
using ATA.HR.Client.Web.APIs.Models.Request;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;

namespace ATA.HR.Client.Web.Pages.AdultStudent;

[Authorize]
public partial class AdultStudentsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    private int TotalAdultCount { get; set; }
    private bool IsLoading { get; set; } = true;
    private List<AdultOutputDto> AllAdults { get; set; } = new();
    public List<AdultOutputDto> NewAddedAdult { get; set; } = new(); //Only if we want to show the recently added Adults
    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private bool ResetPagination { get; set; }
    private bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    private AdultInputDto AdultFilter { get; set; } = new();
    private Subject<string> SearchSubject { get; } = new();

    #endregion

    public AdultUpsertDto AdultUpsertDto { get; set; } = new();
    // public CostAttachmentCommand CostAttachment1 { get; set; } = new();
    private bool IsVisibleDeleteAdultConfirmDialog { get; set; }
    private long DeletingAdultId { get; set; }
    private bool SavingFirstAdult { get; set; } = true;

    // Upload Props
    private bool IsUploading { get; set; }
    private string? UploadingMsg { get; set; }

    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<AdultOutputDto> SelectedItems { get; set; } = Enumerable.Empty<AdultOutputDto>();
    private bool IsVisibleMultipleActionButton => TotalAdultCount > 0;

    // Index
    private TelerikGrid<AdultOutputDto> AdultGridRef { get; set; }

    // Parameter
    [Parameter] public int? FromAddNewAdultPage { get; set; }
    private bool IsNavigatedFromAddNewAdultPage => FromAddNewAdultPage is 1;

    // One way to define relative paths is to put the desired URL here.
    // This can be a full URL such as https://mydomain/myendpoint/save
    private string SaveUrl => ToAbsoluteUrl("api/v1/uploader/upload-file"); //TODO: Rezvan.FileManagement Upload API

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
            var adultStudents = await APIs.GetAdultStudents(AdultFilter);

            if (adultStudents.IsSuccess)
            {
                AllAdults = adultStudents.Data.Data;
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
        if (IsNavigatedFromAddNewAdultPage)
            PageOperationType = OperationType.Add;

        StateHasChanged();

        IsLoading = false;

        return base.OnParametersSetAsync();
    }


    private void OnStateInitHandler(GridStateEventArgs<AdultOutputDto> obj)
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

        SelectedItems = Enumerable.Empty<AdultOutputDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

        await AdultGridRef.SetState(AdultGridRef.GetState());
    }

    private async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                AdultGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            AdultFilter.Page = args.Request.Page;
            AdultFilter.PageSize = PageSize;

            var childStudents = await APIs.GetAdultStudents(AdultFilter);

            if (childStudents.IsSuccess)
            {
                AllAdults = childStudents.Data.Data;

                //args.Data = IsNavigatedFromAddNewAdultPage ? NewAddedCosts : AllMyCosts;

                args.Data = AllAdults;

                args.Total = TotalAdultCount = childStudents.Data.TotalCount;
                //IsNavigatedFromAddNewChildPage
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

        if (AdultFilter.SearchTerm != searchTerm)
            AdultFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AdultFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var cost = (AdultOutputDto)obj.Item;

        //if (cost.FlowCurrentStateTag == InsuranceStateTag.CostEdit.ToString())
        //    obj.Class = "cost-flaw-row";
    }

    private void OpenAddNewAdultPage()
    {
        NavigationManager.NavigateTo(PageUrls.AddAdultFormPage());
    }

    private async Task<DownloadFileResult> ExportCostsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var childrenExcelFilter = AdultFilter.SerializeToJson()!.DeserializeToModel<AdultInputDto>();

            childrenExcelFilter.PageSize = Int32.MaxValue;
            childrenExcelFilter.Page = 1;

            var apiResult = await APIs.GetAdultStudents(childrenExcelFilter);

            if (apiResult.IsSuccess is false || apiResult.Data.TotalCount == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                var reportData = apiResult.Data.Data;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"لیست بزرگسالان")
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

    private void OpenEditAdultPage(int adultId)
    {
        NavigationManager.NavigateTo(PageUrls.EditAdultFormPage(adultId));
    }

    private void RemoveAttachment1()
    {
        //var attachmentIndex = SetCostCommand.CostAttachments.IndexOf(CostAttachment1);

        //SetCostCommand.CostAttachments.RemoveAt(attachmentIndex);

        //CostAttachment1 = new();
    }

    

    private void OpenAdultPrintPage(long adultId)
    {
        NavigationManager.NavigateTo(PageUrls.AdultPrintPage((int)adultId));
    }

    private async Task PrintFilteredStudents()
    {
        //List<CostDto> costsToPrint;

        //if (IsMultiActionOnCustomSelection is false)
        //{
        //    costsToPrint = AllMyCosts;
        //}
        //else
        //{
        //    costsToPrint = SelectedItems.ToList();
        //}

        //AppData.CostsToPrint = costsToPrint;

        //var currentUserId = await AuthenticationStateTask.GetUserId();

        //var hasSignature = await HttpClient.User().HasCurrentUserActiveSignature();

        //if (hasSignature is false)
        //{
        //    NotificationService.Toast(NotificationType.Error, "امضای شما تعریف نشده است و نمی‌توانید پرینت هزینه‌های خود را دریافت نمایید");

        //    return;
        //}

        //var currentUser = await HttpClient.User().GetUserById(currentUserId.ToInt());

        //AppData.CostsPrintInfo = (currentUser.FullName, currentUser.PersonnelCode.ToString(), currentUser.SignatureUrl);

        //NavigationManager.NavigateTo(PageUrls.MyCostsPrintPage);
    }

    private void ShowDeleteAdultConfirmDialog(long adultId)
    {
        DeletingAdultId = adultId;

        IsVisibleDeleteAdultConfirmDialog = true;
    }

    private async Task DeleteAdult(long deletingAdultId)
    {
        try
        {
            await APIs.DeleteAdult(deletingAdultId);

            NotificationService.Toast(NotificationType.Success, "حذف بزرگسال با موفقیت انجام شد");
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


    private Task OnYearFilterChanged(object arg)
    {
        throw new NotImplementedException();
    }
}