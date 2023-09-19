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

namespace ATA.HR.Client.Web.Pages.ChildStudent;

[Authorize]
public partial class ChildStudentsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    private int TotalChildrenCount { get; set; }
    private bool IsLoading { get; set; } = true;
    private List<ChildrenOutputDto> AllChildren { get; set; } = new();
    public List<ChildrenOutputDto> NewAddedChildren { get; set; } = new(); //Only if we want to show the recently added students
    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private bool ResetPagination { get; set; }
    private bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    private ChildrenInputDto ChildrenFilter { get; set; } = new();
    private Subject<string> SearchSubject { get; } = new();

    #endregion

    public ChildUpsertDto ChildUpsertDto { get; set; } = new();
    // public CostAttachmentCommand CostAttachment1 { get; set; } = new();
    private bool IsVisibleDeleteChildConfirmDialog { get; set; }
    private long DeletingChildId { get; set; }
    private bool SavingFirstChild { get; set; } = true;

    // Upload Props
    private bool IsUploading { get; set; }
    private string? UploadingMsg { get; set; }

    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<ChildrenOutputDto> SelectedItems { get; set; } = Enumerable.Empty<ChildrenOutputDto>();
    private bool IsVisibleMultipleActionButton => TotalChildrenCount > 0;

    // Index
    private TelerikGrid<ChildrenOutputDto> ChildrenGridRef { get; set; }

    // Parameter
    [Parameter] public int? FromAddNewChildPage { get; set; }
    private bool IsNavigatedFromAddNewChildPage => FromAddNewChildPage is 1;

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
            var childStudents = await APIs.GetChildStudents(ChildrenFilter);

            if (childStudents.IsSuccess)
            {
                AllChildren = childStudents.Data.Data;
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
        if (IsNavigatedFromAddNewChildPage)
            PageOperationType = OperationType.Add;

        StateHasChanged();

        IsLoading = false;

        return base.OnParametersSetAsync();
    }


    private void OnStateInitHandler(GridStateEventArgs<ChildrenOutputDto> obj)
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

        SelectedItems = Enumerable.Empty<ChildrenOutputDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

        await ChildrenGridRef.SetState(ChildrenGridRef.GetState());
    }

    private async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                ChildrenGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            ChildrenFilter.Page = args.Request.Page;
            ChildrenFilter.PageSize = PageSize;

            var childStudents = await APIs.GetChildStudents(ChildrenFilter);

            if (childStudents.IsSuccess)
            {
                AllChildren = childStudents.Data.Data;

                //args.Data = IsNavigatedFromAddNewChildPage ? NewAddedCosts : AllMyCosts;

                args.Data = AllChildren;

                args.Total = TotalChildrenCount = childStudents.Data.TotalCount;
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

        if (ChildrenFilter.SearchTerm != searchTerm)
            ChildrenFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(ChildrenFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var cost = (ChildrenOutputDto)obj.Item;

        //if (cost.FlowCurrentStateTag == InsuranceStateTag.CostEdit.ToString())
        //    obj.Class = "cost-flaw-row";
    }

    private void OpenAddNewChildPage()
    {
        NavigationManager.NavigateTo(PageUrls.AddChildFormPage());
    }

    private async Task<DownloadFileResult> ExportCostsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var childrenExcelFilter = ChildrenFilter.SerializeToJson()!.DeserializeToModel<ChildrenInputDto>();

            childrenExcelFilter.PageSize = Int32.MaxValue;
            childrenExcelFilter.Page = 1;

            var apiResult = await APIs.GetChildStudents(childrenExcelFilter);

            if (apiResult.IsSuccess is false || apiResult.Data.TotalCount == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                var reportData = apiResult.Data.Data;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"لیست کودکان")
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

    private void OpenEditChildPage(int childId)
    {
        NavigationManager.NavigateTo(PageUrls.EditChildFormPage(childId));
    }

    private void RemoveAttachment1()
    {
        //var attachmentIndex = SetCostCommand.CostAttachments.IndexOf(CostAttachment1);

        //SetCostCommand.CostAttachments.RemoveAt(attachmentIndex);

        //CostAttachment1 = new();
    }

    

    private void OpenChildPrintPage(long childId)
    {
        NavigationManager.NavigateTo(PageUrls.ChildPrintPage((int)childId));
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

    private void ShowDeleteChildConfirmDialog(long childId)
    {
        DeletingChildId = childId;

        IsVisibleDeleteChildConfirmDialog = true;
    }

    private async Task DeleteChild(long deletingChildId)
    {
        try
        {
            await APIs.DeleteChild(deletingChildId);

            NotificationService.Toast(NotificationType.Success, "حذف کودک با موفقیت انجام شد");
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