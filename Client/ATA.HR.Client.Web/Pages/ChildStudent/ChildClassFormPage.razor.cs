using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
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
using ATABit.Shared;
using DNTPersianUtils.Core;

namespace ATA.HR.Client.Web.Pages.ChildStudent;

[Authorize]
public partial class ChildClassFormPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Add;

    private ChildClassUpsertDto Child { get; set; } = new();

    private TelerikGrid<ChildDeliverAddDto> ChildPickersGridRef { get; set; }

    public List<SelectListItem> ChildrenSource { get; set; } = new();
    public List<SelectListItem> ClassesSource { get; set; } = new();
    public List<SelectListItem> TeachersSource { get; set; } = new();

    // Parameter
    [Parameter] public int? ChildClassId { get; set; }

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
            var children = await APIs.GetChildrenItems();
            foreach (var child in children.Data)
            {
                ChildrenSource.Add(new SelectListItem
                {
                    Text = child.Text,
                    Value = child.Value.ToString()
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
        }
        finally
        {
            IsLoading = true; //Will false in OnParamSet event
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ChildClassId.HasValue)
        {
            PageOperationType = OperationType.Edit;

            var result = await APIs.GetChildClassByIdForForm(ChildClassId.Value!);

            if (result.IsSuccess)
            {
                Child = result.Data;
                Child.FromDateJalali = Child.From.ToJalaliString();
                Child.ToDateJalali = Child.To.ToJalaliString();
                //Child.ChildMoreInfo.ChildLiveWithSelectedValue = Child.ChildMoreInfo.ChildLiveWith?.ToString("D");
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

    private async Task OnChildClassSubmit()
    {
        IsLoading = true;

        try
        {
            Child.From = Child.FromDateJalali!.ToDateTime();
            Child.To = Child.ToDateJalali!.ToDateTime();
            //Child.ChildMoreInfo.PhysicalCondition = Child.ChildMoreInfo.PhysicalConditionSelectedValue.IsNotNullOrEmpty()
            //    ? (PhysicalConditionEnum)Child.ChildMoreInfo.PhysicalConditionSelectedValue!.ToInt()
            //    : null;

            if (PageOperationType is OperationType.Add)
            {
                var apiResult = await APIs.CreateChildClass(Child);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "کلاس جدید برای کودک با موفقیت ثبت شد");
            }
            else if (PageOperationType is OperationType.Edit)
            {
                var apiResult = await APIs.UpdateChildClass(Child);

                if (apiResult.IsSuccess)
                    NotificationService.Toast(NotificationType.Success, "کلاس کودک با موفقیت ویرایش شد");
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

    private void OpenChildStudentsPage()
    {
        NavigationManager.NavigateTo(PageUrls.ChildClassPage);
    }
}