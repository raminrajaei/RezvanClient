using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos;
using ATABit.Helper.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Pages.Setting.User;

[Authorize]
public partial class ConfirmManagerIdentityPage
{
    // Props
    private bool IsLoading { get; set; }
    private List<DirectManagerReadDto> DirectManagers { get; set; } = new();
    private string? SmsConfirmationCode { get; set; }
    private bool IsVisibleConfirmSection { get; set; }

    // Injects
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }


    // Cascading Parameters
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            DirectManagers = await HttpClient.User().ContractsGetDirectManagers(cancellationToken: cancellationToken);

            // Check current user should be a direct manager, otherwise redirect to Home (Dashboard)

            var userId = await AuthenticationStateTask.GetUserId();

            if (DirectManagers.Any(dm => dm.ConfirmerUserId.ToString() == userId) is false)
            {
                NavigationManager.NavigateTo(PageUrls.Dashboard);

                return;
            }

            // Check there be no valid DidDirectManagerConfirmBySms cookie, if there is, redirect to Home (Dashboard)

            var cookie = await JsRuntime.GetCookieAsync(AppConstants.AuthToken.DidDirectManagerConfirmBySmsCookie);

            if (cookie.IsNotNullOrEmpty())
            {
                NavigationManager.NavigateTo(PageUrls.Dashboard);

                return;
            }
        }
        finally
        {
            IsLoading = false;
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    // Methods

    private async Task ConfirmMe()
    {
        IsLoading = true;

        try
        {
            await HttpClient.User().ConfirmMeAsDirectManager(SmsConfirmationCode);

            NotificationService.Toast(NotificationType.Success, "کد ورود تایید شد. در حال انتقال به صفحه اصلی");

            await JsRuntime.SetCookieAsync(AppConstants.AuthToken.DidDirectManagerConfirmBySmsCookie,
                "hqifbkgfnmvbdyegje", AppConstants.AuthToken.Lifetime.TotalMinutes);

            StateHasChanged();

            await Task.Delay(2000);

            NavigationManager.NavigateTo(PageUrls.Dashboard);
        }
        catch
        {
            // Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private async Task SendCodeToMe()
    {
        IsLoading = true;

        try
        {
            await HttpClient.User().SendManagerIdentityConfirmSMS();

            IsVisibleConfirmSection = true;
        }
        catch
        {
            // Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}