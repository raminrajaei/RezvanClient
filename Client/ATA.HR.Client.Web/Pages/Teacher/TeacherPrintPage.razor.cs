using System.Threading;
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Models.Response;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models;
using ATA.HR.Client.Web.Models.AppSettings;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ATA.HR.Client.Web.Pages.Teacher;

public partial class TeacherPrintPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private bool ShowResult { get; set; }

    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private TeacherDetailDto Teacher { get; set; } = new();

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public IRezvanAPIs APIs { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public ClientAppSettings ClientAppSettings { get; set; }

    [Parameter] 
    public int TeacherId { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            if (TeacherId == default)
            {
                await NotificationService.AlertAsync(NotificationType.Error, "پارامتر ورودی معتبر نیست");

                return;
            }

            var result = await APIs.GetTeacherDetail(TeacherId);

            if (result.IsSuccess)
            {
                if (result.Data == null)
                {
                    await NotificationService.AlertAsync(NotificationType.Error, "پارامتر ورودی معتبر نیست. هیچ مدرسی پیدا نشد");

                    return;
                }

                Teacher = result.Data;

                var activities = new List<string>();
                if (Teacher.IsMoalem)
                {
                    activities.Add("معلم");
                }
                if (Teacher.IsModares)
                {
                    activities.Add("مدرس");
                }
                if (Teacher.IsMomtahen)
                {
                    activities.Add("ممتحن");
                }
                if (Teacher.IsMorabi)
                {
                    activities.Add("مربی");
                }

                Teacher.Activities = string.Join(",", activities);
            }
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OpenTeachersPage()
    {
        NavigationManager.NavigateTo(PageUrls.TeachersPage);
    }

    //private async Task PrintDoc()
    //{
    //    await JsRuntimeExtensions.Print();
    //}
}

