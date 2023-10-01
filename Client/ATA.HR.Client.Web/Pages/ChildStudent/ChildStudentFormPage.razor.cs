using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;
using ATA.HR.Client.Web.APIs.Enums;
using ATA.HR.Client.Web.APIs.Models.Response;
using Telerik.Blazor.Components;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.Models.AppSettings;
using ATABit.Helper.Extensions;
using Telerik.Blazor;

namespace ATA.HR.Client.Web.Pages.ChildStudent;

[Authorize]
public partial class ChildStudentFormPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Add;

    private ChildUpsertDto Child { get; set; } = new();

    private TelerikGrid<ChildDeliverAddDto> ChildPickersGridRef { get; set; }

    private List<string> HabitsList { get; } = new List<string>
    {
        "مکیدن انگشت",
        "ناخن جویدن",
        "شب ادراری",
        "لکنت یا اشکال در تکلم",
        "گوشه گیری"
    };

    private IEnumerable<string> _selectedHabits = new string[] { };

    // Upload Props
    private bool IsUploading { get; set; }
    private string? UploadingMsg { get; set; }

    // Parameter
    [Parameter] public int? ChildId { get; set; }

    // One way to define relative paths is to put the desired URL here.
    // This can be a full URL such as https://mydomain/myendpoint/save
    private string SaveUrl => ToAbsoluteUrl("/api/FileManager/child-photo"); 

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
        if (ChildId.HasValue)
        {
            PageOperationType = OperationType.Edit;

            var result = await APIs.GetChildByIdForForm(ChildId.Value!);

            if (result.IsSuccess)
            {
                Child = result.Data;
                Child.BirthDateJalali = Child.BirthDate.ToJalaliString();
                Child.FatherBirthDateJalali = Child.FatherBirthDate?.ToJalaliString();
                Child.MotherBirthDateJalali = Child.MotherBirthDate?.ToJalaliString();
                Child.ChildMoreInfo.ChildLiveWithSelectedValue = Child.ChildMoreInfo.ChildLiveWith?.ToString("D");
                Child.ChildMoreInfo.PhysicalConditionSelectedValue = Child.ChildMoreInfo.PhysicalCondition?.ToString("D");
                Child.ChildMoreInfo.DidChildHaveAccidentInThePast = string.IsNullOrWhiteSpace(Child.ChildMoreInfo.PlaceOfInjury) is false;
                _selectedHabits = Child.ChildMoreInfo.SpecialHabits.Split(",");
            }
        }

        StateHasChanged();

        IsLoading = false;

        await base.OnParametersSetAsync();
    }

    private void OnStateInitHandler(GridStateEventArgs<ChildrenOutputDto> obj)
    {

    }

    // Methods

    private async Task RebindChildPickersGrid()
    {
        // Rebind RequestSamples Grid
        await ChildPickersGridRef.SetState(ChildPickersGridRef.GetState());
    }

    private string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{ClientAppSettings.UrlSettings!.RezvanFileManagerURL}{relativeUrl}";
    }

    private string ToDocFullURL(string filePath)
    {
        return $"{filePath}";
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private async Task OnChildStudentSubmit()
    {
        if (ValidateUploadingChildImageOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            Child.BirthDate = Child.BirthDateJalali!.ToDateTime();
            Child.FatherBirthDate = Child.FatherBirthDateJalali?.ToDateTime();
            Child.MotherBirthDate = Child.MotherBirthDateJalali?.ToDateTime();
            Child.ChildMoreInfo.ChildLiveWith = Child.ChildMoreInfo.ChildLiveWithSelectedValue.IsNotNullOrEmpty()
                ? (ChildLiveWithEnum)Child.ChildMoreInfo.ChildLiveWithSelectedValue!.ToInt()
                : null;
            Child.ChildMoreInfo.PhysicalCondition = Child.ChildMoreInfo.PhysicalConditionSelectedValue.IsNotNullOrEmpty()
                ? (PhysicalConditionEnum)Child.ChildMoreInfo.PhysicalConditionSelectedValue!.ToInt()
                : null;
            Child.ChildMoreInfo.SpecialHabits = string.Join(",", _selectedHabits);

            if (PageOperationType is OperationType.Add)
            {
                var apiResult = await APIs.CreateChildStudent(Child);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "داوطلب کودک با موفقیت ثبت شد");
            }
            else if (PageOperationType is OperationType.Edit)
            {
                var apiResult = await APIs.UpdateChildStudent(Child);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "داوطلب کودک با موفقیت ویرایش شد");
            }

            OpenChildStudentsPage();
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

    private bool ValidateUploadingChildImageOnSubmit()
    {
        if (false /*ChildAddDto.url.Count == 0*/)
        {
            NotificationService.Toast(NotificationType.Error, "تصویر داوطلب بارگزاری نشده است");

            return false;
        }

        return true;
    }

    #region Uploader Methods

    private void OnFileSelectToUpload(UploadSelectEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Child.IdNo))
        {
            NotificationService.Toast(NotificationType.Error, "قبل از افزودن عکس کودک، شماره شناسنامه وی را نمایید");

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

        if(uploadResult.IsSuccess)
        {
            Child.PhotoPath = uploadResult.Data;

            NotificationService.Toast(NotificationType.Success, "عکس با موفقیت بارگزاری شد");
        }

        IsUploading = false;
        UploadingMsg = null;
    }

    private void OnUploadHandler(UploadEventArgs e)
    {
        e.RequestHeaders.Add("Identifier", Child.IdNo);
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

    private void RemoveChildImage()
    {
        Child.PhotoPath = null;
    }

    private void OpenChildStudentsPage()
    {
        NavigationManager.NavigateTo(PageUrls.ChildrenPage);
    }

    private async Task CreateHandler(GridCommandEventArgs args)
    {
        ChildDeliverAddDto picker = (ChildDeliverAddDto)args.Item;

        Child.ChildDelivers.Add(picker);

        NotificationService.Toast(NotificationType.Success, "تحویل گیرنده اضافه شد");

        await RebindChildPickersGrid();
    }

    private async Task DeleteHandler(GridCommandEventArgs args)
    {
        ChildDeliverAddDto picker = (ChildDeliverAddDto)args.Item;

        Child.ChildDelivers.Remove(picker);

        NotificationService.Toast(NotificationType.Success, "تحویل گیرنده حذف شد");

        await RebindChildPickersGrid();
    }
}