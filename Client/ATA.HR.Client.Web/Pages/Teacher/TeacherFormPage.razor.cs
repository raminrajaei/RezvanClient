using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;
using ATA.HR.Client.Web.APIs.Models.Response;
using Telerik.Blazor.Components;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.Models.AppSettings;
using ATABit.Helper.Extensions;
using Telerik.Blazor;
using ATA.HR.Client.Web.APIs.Enums;

namespace ATA.HR.Client.Web.Pages.Teacher;

[Authorize]
public partial class TeacherFormPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Add;

    private TeacherUpsertDto Teacher { get; set; } = new();

    private IEnumerable<string> _selectedHabits = new string[] { };

    // Upload Props
    private bool IsUploading { get; set; }
    private string? UploadingMsg { get; set; }

    // Parameter
    [Parameter] public int? TeacherId { get; set; }

    // One way to define relative paths is to put the desired URL here.
    // This can be a full URL such as https://mydomain/myendpoint/save
    private string SaveUrl => ToAbsoluteUrl("/api/FileManager/teacher-photo");

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IRezvanAPIs APIs { get; set; }
    [Inject] public ClientAppSettings ClientAppSettings { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {

        }
        finally
        {
            IsLoading = true; //Will false in OnParamSet event
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (TeacherId.HasValue)
        {
            PageOperationType = OperationType.Edit;

            var result = await APIs.GetTeacherByIdForForm(TeacherId.Value!);

            if (result.IsSuccess)
            {
                Teacher = result.Data;
                Teacher.BirthDateJalali = Teacher.BirthDate.ToJalaliString();
                Teacher.GenderSelectedValue = Teacher.Gender?.ToString("D");
                Teacher.MaritalStatusSelectedValue = Teacher.MaritalStatus?.ToString("D");
            }
        }

        StateHasChanged();

        IsLoading = false;

        await base.OnParametersSetAsync();
    }

    private void OnStateInitHandler(GridStateEventArgs<TeacherOutputDto> obj)
    {

    }

    // Methods

    private string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{ClientAppSettings.UrlSettings!.RezvanFileManagerURL}{relativeUrl}";
    }

    private string ToDocFullURL(string filePath)
    {
        return $"{ClientAppSettings.UrlSettings!.RezvanFileManagerURL}/{filePath}";
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private async Task OnTeacherSubmit()
    {
        if (ValidateUploadingTeacherImageOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            Teacher.BirthDate = Teacher.BirthDateJalali!.ToDateTime();

            Teacher.Gender = Teacher.GenderSelectedValue.IsNotNullOrEmpty()
                ? (GenderEnum)Teacher.GenderSelectedValue!.ToInt()
                : null;

            Teacher.MaritalStatus = Teacher.MaritalStatusSelectedValue.IsNotNullOrEmpty()
                ? (MaritalStatusEnum)Teacher.MaritalStatusSelectedValue!.ToInt()
                : null;

            if (PageOperationType is OperationType.Add)
            {
                var apiResult = await APIs.CreateTeacher(Teacher);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "مدرس با موفقیت ثبت شد");
            }
            else if (PageOperationType is OperationType.Edit)
            {
                var apiResult = await APIs.UpdateTeacher(Teacher);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "مدرس با موفقیت ویرایش شد");
            }

            OpenTeachersPage();
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

    private bool ValidateUploadingTeacherImageOnSubmit()
    {
        if (false /*TeacherAddDto.url.Count == 0*/)
        {
            NotificationService.Toast(NotificationType.Error, "تصویر داوطلب بارگزاری نشده است");

            return false;
        }

        return true;
    }

    #region Uploader Methods

    private void OnFileSelectToUpload(UploadSelectEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Teacher.NationalCode))
        {
            NotificationService.Toast(NotificationType.Error, "قبل از افزودن عکس مدرس، شماره ملی وی را نمایید");

            e.IsCancelled = true;

            return;
        }

        // Handle client side validations
        if (e.Files.Count != 1)
            throw new Exception("آپلود چند فایل قابل پذیرش نیست");

        var file = e.Files.Single();

        var allowedExtensions = new List<string>
        {
            ".jpg", ".png", ".jpeg", //Images
            //".pdf", //PDF
            //".xls", ".xlsx" //Excel
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
    }

    private void OnFileSuccessUpload(UploadSuccessEventArgs e)
    {
        if (e.Operation == UploadOperationType.Remove)
            throw new Exception("We do not have remove file functionality");

        var uploadResult = e.Request.ResponseText.DeserializeToModel<ApiResult<string>>();

        if (uploadResult.IsSuccess)
        {
            Teacher.PhotoPath = uploadResult.Data;

            NotificationService.Toast(NotificationType.Success, "عکس با موفقیت بارگزاری شد");
        }

        IsUploading = false;
        UploadingMsg = null;
    }

    private void OnUploadHandler(UploadEventArgs e)
    {
        e.RequestHeaders.Add("Identifier", Teacher.NationalCode);
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

    private void RemoveTeacherImage()
    {
        Teacher.PhotoPath = null;
    }

    private void OpenTeachersPage()
    {
        NavigationManager.NavigateTo(PageUrls.TeachersPage);
    }
}