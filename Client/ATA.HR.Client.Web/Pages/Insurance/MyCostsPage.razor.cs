using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Enums;
using ATA.HR.Client.Web.APIs.HRInsuranceModels;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos.Document;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Core.Exceptions;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using DNTPersianUtils.Core;
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
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;

namespace ATA.HR.Client.Web.Pages.Insurance;

[Authorize]
public partial class MyCostsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    public int TotalCostsCounts { get; set; }
    private bool IsLoading { get; set; } = true;
    public List<CostDto> AllMyCosts { get; set; } = new();
    public List<CostDto> NewAddedCosts { get; set; } = new();
    public string TotalCostsAmount => AllMyCosts.Sum(c => c.PaidAmount).ToCurrencyStringFormat().ToPersianNumbers();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public GetCostsQuery CostsQuery { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> MyActiveInsuredsSource { get; set; } = new();
    private bool IsAllowedToAddCost => MyActiveInsuredsSource.Count > 0;
    private List<PolicyObligationDto> ActivePolicyObligations { get; set; } = new();
    private List<SelectListItem> ActivePolicyObligationsSource { get; set; } = new();
    public SetCostCommand SetCostCommand { get; set; } = new();
    public CostAttachmentCommand CostAttachment1 { get; set; } = new();
    public CostAttachmentCommand CostAttachment2 { get; set; } = new();
    public CostAttachmentCommand CostAttachment3 { get; set; } = new();
    public bool IsVisibleSelectObligationWindow { get; set; }
    private bool IsVisibleDeleteCostConfirmDialog { get; set; }
    private int DeletingCostId { get; set; }
    private string? CurrentUserPersonnelCode { get; set; }
    private bool SavingFirstCost { get; set; } = true;

    // Upload Props
    public UploadRequestData UploadRequestData { get; set; } = new();
    public bool IsUploading { get; set; }
    public string? UploadingMsg { get; set; }

    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<CostDto> SelectedItems { get; set; } = Enumerable.Empty<CostDto>();
    public bool IsVisibleMultipleActionButton => TotalCostsCounts > 0;

    // Index
    private TelerikGrid<CostDto> CostsGridRef { get; set; }

    // Parameter
    [Parameter] public int? FromAddNewCostPage { get; set; }
    private bool IsNavigatedFromAddNewCostPage => FromAddNewCostPage is 1;

    // One way to define relative paths is to put the desired URL here.
    // This can be a full URL such as https://mydomain/myendpoint/save
    public string SaveUrl => ToAbsoluteUrl("api/v1/uploader/upload-file");

    // Inject
    [Inject] public AppData AppData { get; set; }
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
            AppData.CostsToPrint = new();

            var myInsureds = await CoreAPIs.Insurance_GetMyInsureds();
            MyActiveInsuredsSource = myInsureds.Where(i => i.IsActive).Select(i => new SelectListItem(i.FullName, i.Id.ToString())).ToList();

            ActivePolicyObligations = await CoreAPIs.Insurance_GetActivePolicyObligations();
            ActivePolicyObligationsSource = ActivePolicyObligations
                .Select(o => new SelectListItem(o.ObligationTitle, o.Id.ToString()))
                .ToList();

            CurrentUserPersonnelCode = await AuthenticationStateTask.GetPersonnelCode();

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
        if (IsNavigatedFromAddNewCostPage)
            PageOperationType = OperationType.Add;

        StateHasChanged();

        IsLoading = false;

        return base.OnParametersSetAsync();
    }


    private void OnStateInitHandler(GridStateEventArgs<CostDto> obj)
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

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        SelectedItems = Enumerable.Empty<CostDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

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

            AllMyCosts = await CoreAPIs.Insurance_GetMyCosts(CostsQuery);

            args.Data = IsNavigatedFromAddNewCostPage ? NewAddedCosts : AllMyCosts;

            args.Total = TotalCostsCounts = IsNavigatedFromAddNewCostPage
            ? NewAddedCosts.Count
            : AllMyCosts.Count;  //(int)(context.TotalCount ?? 0);
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

        if (CostsQuery.SearchTerm != searchTerm)
            CostsQuery.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(CostsQuery.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var cost = (CostDto)obj.Item;

        if (cost.FlowCurrentStateTag == InsuranceStateTag.CostEdit.ToString())
            obj.Class = "cost-flaw-row";
    }

    private void ChangeToAddMode()
    {
        SetCostCommand = new SetCostCommand();

        PageOperationType = OperationType.Add;
    }

    private async Task OnCostSubmit()
    {
        if (ValidateCostOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            if (PageOperationType is OperationType.Add)
            {
                var newCost = await CoreAPIs.Insurance_AddCost(SetCostCommand);

                NewAddedCosts.Add(newCost);

                NotificationService.Toast(NotificationType.Success, "هزینه با موفقیت ثبت شد");
            }
            else if (PageOperationType is OperationType.Edit)
            {
                await CoreAPIs.Insurance_EditCost(SetCostCommand);

                NotificationService.Toast(NotificationType.Success, "هزینه با موفقیت ویرایش شد");
            }

            ChangeToFilterMode();

            await RebindGrid(false);

            SavingFirstCost = false;

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
        if (SetCostCommand.CostAttachments.Count == 0)
        {
            NotificationService.Toast(NotificationType.Error, "هیچ پیوستی بارگزاری نشده است. حداقل یک پیوست بارگزاری نمایید.");

            return false;
        }

        return true;
    }

    #region Uploader Methods

    private void OnFileSelectToUpload(UploadSelectEventArgs e, int attachmentNo)
    {
        if (SetCostCommand.InsuredId.HasValue is false || SetCostCommand.PolicyObligationId.HasValue is false ||
            SetCostCommand.PaidAmount.HasValue is false || string.IsNullOrWhiteSpace(SetCostCommand.Title))
        {
            NotificationService.Toast(NotificationType.Error, "قبل از افزودن پیوست، سایر مشخصات هزینه را وارد نمایید");

            e.IsCancelled = true;

            return;
        }

        bool titleEntered = true;

        switch (attachmentNo)
        {
            case 1:
                {
                    if (string.IsNullOrWhiteSpace(CostAttachment1.Title))
                        titleEntered = false;
                    break;
                }
            case 2:
                {
                    if (string.IsNullOrWhiteSpace(CostAttachment2.Title))
                        titleEntered = false;
                    break;
                }
            case 3:
                {
                    if (string.IsNullOrWhiteSpace(CostAttachment3.Title))
                        titleEntered = false;
                    break;
                }
            default:
                throw new Exception("حاجی این شماره پیوست چیه همچین چیزی نداریم");
        }

        if (titleEntered is false)
        {
            NotificationService.Toast(NotificationType.Error, "قبل از افزودن پیوست، عنوان آن را وارد نمایید");

            e.IsCancelled = true;

            return;
        }

        UploadRequestData = new UploadRequestData
        {
            FolderName = "insurance-cost-attachments",
            SubfolderName = CurrentUserPersonnelCode,
            FileName = $"{DateTime.Now:yyyy-MM-dd}"
        };

        // Handle client side validations
        if (e.Files.Count != 1)
            throw new Exception("آپلود چند فایل قابل پذیرش نیست");

        var file = e.Files.Single();

        var allowedExtensions = new List<string>
        {
            ".jpg", ".png", ".jpeg", //Images
            ".pdf", //PDF
            ".xls", ".xlsx" //Excel
        };

        Console.WriteLine($@"Allowed Extensions: {allowedExtensions.SerializeToJson()}");

        if (allowedExtensions.Contains(file.Extension.ToLower()) is false)
        {
            NotificationService.Toast(NotificationType.Error, "فایل با این فرمت مجاز به آپلود نیست. فقط فایل‌های تصویری jpeg, jpg, png و فایل pdf و اکسل مجاز می‌باشند.");
            e.IsCancelled = true;
            return;
        }

        var maxAllowedSize = AppConstants.UploadLimits.DocMaxSizeAllowedToUploadInKiloBytes * 1024;

        if (file.Size > maxAllowedSize)
        {
            NotificationService.Toast(NotificationType.Error, "حجم فایل بیشتر از مقدار مجاز 10 مگابایت می‌باشد");
            e.IsCancelled = true;
            return;
        }

        IsUploading = true;

        //UploadingMsg = $"Uploading {file.Size.Bytes().Humanize("0.0")}";

        switch (attachmentNo)
        {
            case 1:
                CostAttachment1.FileExtension = file.Extension;
                CostAttachment1.FileType = GetFileTypeFromExtension(file.Extension);
                break;
            case 2:
                CostAttachment2.FileExtension = file.Extension;
                CostAttachment2.FileType = GetFileTypeFromExtension(file.Extension);
                break;
            case 3:
                CostAttachment3.FileExtension = file.Extension;
                CostAttachment3.FileType = GetFileTypeFromExtension(file.Extension);
                break;
            default:
                throw new Exception("حاجی این شماره پیوست چیه همچین چیزی نداریم");
        }
    }

    private void OnUploadHandler(UploadEventArgs e)
    {
        e.RequestHeaders.Add(nameof(UploadRequestData.FolderName), UploadRequestData.FolderName);
        e.RequestHeaders.Add(nameof(UploadRequestData.FileName), UploadRequestData.FileName);
        e.RequestHeaders.Add(nameof(UploadRequestData.SubfolderName), UploadRequestData.SubfolderName);
    }

    private void OnFileSuccessUpload(UploadSuccessEventArgs e, int attachmentNo)
    {
        if (e.Operation == UploadOperationType.Remove)
            throw new DomainLogicException("We do not have remove file functionality");

        var uploadResult = e.Request.ResponseText.DeserializeToModel<UploadedFileResult>();

        //PersonnelDocument.Identifier = uploadResult.Identifier;

        switch (attachmentNo)
        {
            case 1:
                CostAttachment1.Url = uploadResult.FileUrl;
                SetCostCommand.CostAttachments.Add(CostAttachment1);
                break;
            case 2:
                CostAttachment2.Url = uploadResult.FileUrl;
                SetCostCommand.CostAttachments.Add(CostAttachment2);
                break;
            case 3:
                CostAttachment3.Url = uploadResult.FileUrl;
                SetCostCommand.CostAttachments.Add(CostAttachment3);
                break;
        }

        NotificationService.Toast(NotificationType.Success, "پیوست ثبت شد");

        IsUploading = false;
        UploadingMsg = null;
    }

    private static string? GetFileTypeFromExtension(string? fileExtension)
    {
        switch (fileExtension)
        {
            case ".jpg":
            case ".png":
            case ".jpeg":
                return "Image";

            case ".pdf":
                return "PDF";

            case ".xsl":
            case ".xlsx":
                return "Excel";

            default: throw new ArgumentException("نوع فایل در هیچکدام از نوع فایل‌های مشخص شده قرار نمی‌گیرد");
        }
    }
    #endregion

    private void OnSelectObligation(int poId)
    {
        SetCostCommand.PolicyObligationId = poId;

        IsVisibleSelectObligationWindow = false;
    }

    private async Task<DownloadFileResult> ExportCostsDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var reportData = await CoreAPIs.Insurance_GetMyCosts(CostsQuery);

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

    private async Task ChangeToEditMode(int costId)
    {
        IsLoading = true;

        try
        {
            SetCostCommand = await CoreAPIs.Insurance_GetCostByIdForForm(costId);

            CostAttachment1 = new();
            CostAttachment2 = new();
            CostAttachment3 = new();

            for (int i = 0; i < SetCostCommand.CostAttachments.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        CostAttachment1 = SetCostCommand.CostAttachments[0];
                        break;
                    case 1:
                        CostAttachment2 = SetCostCommand.CostAttachments[1];
                        break;
                    case 3:
                        CostAttachment3 = SetCostCommand.CostAttachments[2];
                        break;
                }
            }

            PageOperationType = OperationType.Edit;
        }
        catch (Exception e)
        {
            // ignored
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void RemoveAttachment1()
    {
        var attachmentIndex = SetCostCommand.CostAttachments.IndexOf(CostAttachment1);

        SetCostCommand.CostAttachments.RemoveAt(attachmentIndex);

        CostAttachment1 = new();
    }

    private void RemoveAttachment2()
    {
        var attachmentIndex = SetCostCommand.CostAttachments.IndexOf(CostAttachment2);

        SetCostCommand.CostAttachments.RemoveAt(attachmentIndex);

        CostAttachment2 = new();
    }

    private void OpenCostFlowFormsPage(int costId)
    {
        NavigationManager.NavigateTo(PageUrls.InsuranceCostFlowFormsPage(costId));
    }

    private async Task PrintFilteredCosts()
    {
        List<CostDto> costsToPrint;

        if (IsMultiActionOnCustomSelection is false)
        {
            costsToPrint = AllMyCosts;
        }
        else
        {
            costsToPrint = SelectedItems.ToList();
        }

        AppData.CostsToPrint = costsToPrint;

        var currentUserId = await AuthenticationStateTask.GetUserId();

        var hasSignature = await HttpClient.User().HasCurrentUserActiveSignature();

        if (hasSignature is false)
        {
            NotificationService.Toast(NotificationType.Error, "امضای شما تعریف نشده است و نمی‌توانید پرینت هزینه‌های خود را دریافت نمایید");

            return;
        }

        var currentUser = await HttpClient.User().GetUserById(currentUserId.ToInt());

        AppData.CostsPrintInfo = (currentUser.FullName, currentUser.PersonnelCode.ToString(), currentUser.SignatureUrl);

        NavigationManager.NavigateTo(PageUrls.MyCostsPrintPage);
    }

    private void ShowDeleteCostConfirmDialog(int costId)
    {
        DeletingCostId = costId;

        IsVisibleDeleteCostConfirmDialog = true;
    }

    private async Task DeleteCost(int deletingCostId)
    {
        try
        {
            await CoreAPIs.Insurance_DeleteCost(deletingCostId);

            NotificationService.Toast(NotificationType.Success, "حذف هزینه با موفقیت انجام شد");
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
}