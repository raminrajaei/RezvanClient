using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos.Document;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Core.Exceptions;
using Bit.Http.Contracts;
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

namespace ATA.HR.Client.Web.Pages.Document;

[Authorize]
public partial class PersonnelDocumentsPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    public int TotalDocumentsCounts { get; set; }
    public string? AvatarPicUrl { get; set; }
    private bool IsLoading { get; set; } = true;
    public List<DocSubCategoryReadDto> AllSubCategories { get; set; } = new();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public PersonnelDocumentFilterArgs PersonnelDocumentFilterArgs { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> FilterSubCategoriesSource { get; set; } = new();
    private List<SelectListItem> FormSubCategoriesSource { get; set; } = new();
    private UserDto? User { get; set; }
    public PersonnelDocumentDto PersonnelDocument { get; set; } = new();
    public bool IsVisibleDeleteDocumentConfirmDialog { get; set; }
    public int DeletingDocumentId { get; set; }

    // Upload Props
    public UploadRequestData UploadRequestData { get; set; }
    public bool IsUploading { get; set; }
    public string? UploadingMsg { get; set; }

    // Index
    private TelerikGrid<PersonnelDocumentReadDto> DocumentsGridRef { get; set; }

    // Parameter
    [Parameter] public int UserId { get; set; }
    [Parameter] public int? DocumentCategory { get; set; }
    [Parameter] public int? SubCategoryId { get; set; }
    [Parameter] public int? IsFromCommitmentLetter { get; set; }
    private bool IsNavigatedFromCommitmentLetterPage => IsFromCommitmentLetter is 1;

    // One way to define relative paths is to put the desired URL here.
    // This can be a full URL such as https://mydomain/myendpoint/save
    public string SaveUrl => ToAbsoluteUrl("api/v1/uploader/upload-file");

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            PersonnelDocumentFilterArgs.UserId = UserId;

            User = await HttpClient.User().GetUserById(UserId, cancellationToken: cancellationToken);

            if (User is null)
            {
                NotificationService.Toast(NotificationType.Error, "کاربر یافت نشد");

                return;
            }

            AvatarPicUrl = User.PictureURL;

            AllSubCategories = await HttpClient.PersonnelDocument().GetAllDocumentsSubCategories(cancellationToken: cancellationToken);

            if (DocumentCategory.HasValue)
            {
                PersonnelDocumentFilterArgs.DocCategorySelectedValue = DocumentCategory.Value.ToString();

                await OnFilterCategoryDropdownChanged(DocumentCategory, false);

                if (SubCategoryId.HasValue)
                    PersonnelDocumentFilterArgs.DocSubCategorySelectedValue = SubCategoryId.Value.ToString();

                await ApplyFilters;
            }

            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<PersonnelDocumentReadDto> obj)
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
    private Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        return DocumentsGridRef.SetState(DocumentsGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                DocumentsGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            args.Data = await HttpClient.PersonnelDocument().GetUserDocuments(PersonnelDocumentFilterArgs, context, cancellationToken);

            args.Total = TotalDocumentsCounts = (int)(context.TotalCount ?? 0);
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

        if (PersonnelDocumentFilterArgs.SearchTerm != searchTerm)
            PersonnelDocumentFilterArgs.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(PersonnelDocumentFilterArgs.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var user = (PersonnelDocumentReadDto)obj.Item;

        //if (user.Dismissed)
        //    obj.Class = "contract-ending-alert";
    }

    private Task OnFilterCategoryDropdownChanged(object docCat, bool applyFilter = true)
    {
        if (docCat is null)
        {
            FilterSubCategoriesSource = new();

            PersonnelDocumentFilterArgs.DocSubCategorySelectedValue = "";

            PersonnelDocumentFilterArgs.DocCategorySelectedValue = "";

            StateHasChanged();

            return ApplyFilters;
        }

        if (docCat.ToString().IsNotNullOrEmpty())
        {
            try
            {
                var selectedFilterCategoryId = docCat.ToString();
                var filterSubCategories = AllSubCategories
                    .Where(c => c.DocumentCategoryValue.ToString() == selectedFilterCategoryId)
                    .ToList();
                FilterSubCategoriesSource.Clear();
                PersonnelDocumentFilterArgs.DocSubCategorySelectedValue = null;
                FilterSubCategoriesSource = filterSubCategories.Select(sc => new SelectListItem(sc.Title, sc.Id.ToString())).ToList();

                PersonnelDocumentFilterArgs.DocSubCategorySelectedValue = null;

                if (applyFilter)
                {
                    return ApplyFilters;
                }

            }
            catch (Exception e)
            {
                NotificationService.AlertGeneralError();
                Console.WriteLine(e);
            }
        }

        return Task.CompletedTask;
    }

    private void ChangeToAddMode()
    {
        PersonnelDocument = new PersonnelDocumentDto { UserId = UserId };

        PageOperationType = OperationType.Add;
    }

    private async Task OnDocumentSubmit()
    {
        if (ValidateDocumentOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            if (PageOperationType is OperationType.Add)
            {
                await HttpClient.PersonnelDocument().AddDocument(PersonnelDocument);

                NotificationService.Toast(NotificationType.Success, "سند با موفقیت ذخیره گردید");

            }
            else
            {
                await HttpClient.PersonnelDocument().EditDocument(new PersonnelDocumentEditDto(PersonnelDocument.Id!.Value!, PersonnelDocument.Description));

                NotificationService.Toast(NotificationType.Success, "سند با موفقیت ویرایش شد");
            }

            await RebindGrid(false);

            if (PageOperationType is OperationType.Add)
                ChangeToAddMode();

            else
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

    private bool ValidateDocumentOnSubmit()
    {
        if (string.IsNullOrWhiteSpace(PersonnelDocument.FilePath))
        {
            NotificationService.Toast(NotificationType.Error, "هیچ سندی بارگزاری نشده است. ابتدا سند را بارگزاری نمایید.");

            return false;
        }

        return true;
    }

    private void OnFormCategoryDropdownChanged(object x)
    {
        if (x is null)
            return;

        if (x.ToString().IsNotNullOrEmpty())
        {
            try
            {
                var selectedFormCategoryId = x.ToString();
                var formSubCategories = AllSubCategories
                    .Where(c => c.DocumentCategoryValue.ToString() == selectedFormCategoryId)
                    .ToList();
                FormSubCategoriesSource.Clear();
                PersonnelDocument.DocumentSubCategorySelectedValue = null;
                FormSubCategoriesSource = formSubCategories.Select(sc => new SelectListItem(sc.Title, sc.Id.ToString())).ToList();
            }
            catch (Exception e)
            {
                NotificationService.AlertGeneralError();
                Console.WriteLine(e);
            }
        }
    }

    #region Uploader Methods

    private void OnFileSelectToUpload(UploadSelectEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PersonnelDocument.DocumentCategorySelectedValue) || string.IsNullOrWhiteSpace(PersonnelDocument.DocumentSubCategorySelectedValue))
        {
            NotificationService.Toast(NotificationType.Error, "ابتدا گروه و زیرگروه سند را مشخص نمایید");

            return;
        }

        UploadRequestData = new UploadRequestData
        {
            FolderName = "personnel-documents",
            SubfolderName = User?.PersonnelCode.ToString(),
            FileName = $"{PersonnelDocument.DocumentCategory}"
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
            return;
        }

        var maxAllowedSize = AppConstants.UploadLimits.DocMaxSizeAllowedToUploadInKiloBytes * 1024;

        if (file.Size > maxAllowedSize)
        {
            NotificationService.Toast(NotificationType.Error, "حجم فایل بیشتر از مقدار مجاز 10 مگابایت می‌باشد");
            return;
        }

        IsUploading = true;

        //UploadingMsg = $"Uploading {file.Size.Bytes().Humanize("0.0")}";

        PersonnelDocument.SizeBytes = file.Size;

        PersonnelDocument.FileExtension = file.Extension;
    }

    private void OnUploadHandler(UploadEventArgs e)
    {
        e.RequestHeaders.Add(nameof(UploadRequestData.FolderName), UploadRequestData.FolderName);
        e.RequestHeaders.Add(nameof(UploadRequestData.FileName), UploadRequestData.FileName);
        e.RequestHeaders.Add(nameof(UploadRequestData.SubfolderName), UploadRequestData.SubfolderName);
    }

    private void OnFileSuccessUpload(UploadSuccessEventArgs e)
    {
        if (e.Operation == UploadOperationType.Remove)
            throw new DomainLogicException("We do not have remove file functionality");

        var uploadResult = e.Request.ResponseText.DeserializeToModel<UploadedFileResult>();

        PersonnelDocument.Identifier = uploadResult.Identifier;
        PersonnelDocument.FilePath = uploadResult.FileUrl;

        IsUploading = false;
        UploadingMsg = null;
    }

    #endregion

    private void OnUserImageLoadFailed()
    {
        if (User == null) return;

        AvatarPicUrl = "/images/layout/user.png";
    }

    private async Task DeleteDocument(int documentId)
    {
        try
        {
            await HttpClient.PersonnelDocument().DeleteDocument(new DeleteDocumentArgs { DocId = documentId });

            NotificationService.Toast(NotificationType.Success, "حذف سند با موفقیت انجام شد");
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

    private async Task ChangeToEditMode(int documentId)
    {
        IsLoading = true;

        try
        {
            PersonnelDocument = await HttpClient.PersonnelDocument().GetDocumentByIdForForm(documentId);

            PageOperationType = OperationType.Edit;
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ShowDeleteDocumentConfirmDialog(int documentIdToDelete)
    {
        DeletingDocumentId = documentIdToDelete;

        IsVisibleDeleteDocumentConfirmDialog = true;
    }

    private void BackButtonHandler()
    {
        if (IsNavigatedFromCommitmentLetterPage)
        {
            // Open CommitmentLettersPage
            NavigationManager.NavigateTo(PageUrls.CommitmentLettersPage);
        }
        else
        {
            // Open AllUsersPage
            NavigationManager.NavigateTo(PageUrls.DocUsersPage);
        }
    }
}