using System.Threading;
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Client.Web.Models.AppSettings;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ATA.HR.Client.Web.Pages.AdultStudent;

[Authorize]
public partial class AdultClassFormPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Add;

    private AdultClassUpsertDto Adult { get; set; } = new();
    
    public List<SelectListItem> AdultsSource { get; set; } = new();
    public List<SelectListItem> ClassesSource { get; set; } = new();
    public List<SelectListItem> TeachersSource { get; set; } = new();

    // Parameter
    [Parameter] public int? AdultClassId { get; set; }

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
            var adultsItems = await APIs.GetAdultsItems();
            foreach (var adult in adultsItems.Data)
            {
                AdultsSource.Add(new SelectListItem
                {
                    Text = adult.Text,
                    Value = adult.Value.ToString()
                });
            }

            var classes = await APIs.GetClassRoomItems(true);
            foreach (var item in classes.Data)
            {
                ClassesSource.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value.ToString()
                });
            }

            var teachers = await APIs.GetTeachersItems();
            foreach (var teacher in teachers.Data)
            {
                TeachersSource.Add(new SelectListItem
                {
                    Text = teacher.Text,
                    Value = teacher.Value.ToString()
                });
            }
        }
        finally
        {
            IsLoading = true; //Will false in OnParamSet event
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (AdultClassId.HasValue)
        {
            PageOperationType = OperationType.Edit;

            var result = await APIs.GetAdultClassByIdForForm(AdultClassId.Value!);

            if (result.IsSuccess)
            {
                Adult = result.Data;
                Adult.FromDateJalali = Adult.From.ToJalaliString();
                Adult.ToDateJalali = Adult.To.ToJalaliString();
                Adult.AdultIdSelectedValue = Adult.AdultId.ToString();
                Adult.TeacherIdSelectedValue = Adult.TeacherId.ToString();
                Adult.ClassRoomIdSelectedValue = Adult.ClassRoomId.ToString();
            }
        }

        StateHasChanged();

        IsLoading = false;

        await base.OnParametersSetAsync();
    }
    
    // Methods
    
    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private async Task OnAdultClassSubmit()
    {
        IsLoading = true;

        try
        {
            Adult.From = Adult.FromDateJalali!.ToDateTime();
            Adult.To = Adult.ToDateJalali!.ToDateTime();
            Adult.AdultId = Adult.AdultIdSelectedValue.ToLong();
            Adult.TeacherId = Adult.TeacherIdSelectedValue!.ToLong();
            Adult.ClassRoomId = Adult.ClassRoomIdSelectedValue!.ToLong();

            if (PageOperationType is OperationType.Add)
            {
                var apiResult = await APIs.CreateAdultClass(Adult);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "کلاس جدید برای بزرگسال با موفقیت ثبت شد");
            }
            else if (PageOperationType is OperationType.Edit)
            {
                var apiResult = await APIs.UpdateAdultClass(Adult);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "کلاس بزرگسال با موفقیت ویرایش شد");
            }

            OpenAdultClassPage();
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

    private void OpenAdultClassPage()
    {
        NavigationManager.NavigateTo(PageUrls.AdultClassPage);
    }
}