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

namespace ATA.HR.Client.Web.Pages.ChildStudent;

public partial class ChildPrintPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private bool ShowResult { get; set; }

    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private ChildDetailDto Child { get; set; } = new();

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public IRezvanAPIs APIs { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public ClientAppSettings ClientAppSettings { get; set; }

    [Parameter] 
    public int ChildId { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            if (ChildId == default)
            {
                await NotificationService.AlertAsync(NotificationType.Error, "پارامتر ورودی معتبر نیست");

                return;
            }

            var result = await APIs.GetChildDetail(ChildId);

            if (result.IsSuccess)
            {
                if (result.Data == null)
                {
                    await NotificationService.AlertAsync(NotificationType.Error, "پارامتر ورودی معتبر نیست. هیچ کودکی پیدا نشد");

                    return;
                }

                Child = result.Data;
            }
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OpenChildrenPage()
    {
        NavigationManager.NavigateTo(PageUrls.ChildrenPage);
    }

    private async Task PrintDoc()
    {
        await JSRuntime.Print();
    }
}

