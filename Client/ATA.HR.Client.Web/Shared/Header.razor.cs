using ATA.Broker.SSOSecurity.Contract;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models.AppSettings;
using ATA.HR.Client.Web.Pages;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos.AppVersioning;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Shared;

public partial class Header
{
    // Props
    public UserDto User { get; set; } = new();
    public string? AvatarPicUrl { get; set; }
    public string? AvatarPicUrlInProfileModal { get; set; }
    public bool IsOpenProfileWindow { get; set; }
    public bool IsOpenNotificationWindow { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsVisibleHangfireDashboards { get; set; }
    public bool IsOpenAppVersioningWindow { get; set; }
    public bool IsOpenAppVersioningHistoryWindow { get; set; }
    public List<AppVersioningDto> VersionChangeList { get; set; } = new();
    public List<AppVersioningDto> VersioningHistory { get; set; } = new();
    public string AppLatestVersion { get; set; }
    public bool HasCurrentUserActiveSignature { get; set; }


    //  Injects
    [Inject] public ILocalStorageService LocalStorageService { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IATASSOClientService SsoClientService { get; set; }
    [Inject] public ClientAppSettings ClientAppSettings { get; set; }
    [Inject] public IWebAssemblyHostEnvironment HostEnvironment { get; set; }

    // Cascading Parameters
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life cycle
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        var isAuthenticated = await AuthenticationStateTask.IsAuthenticated();

        // Only Login page gets into this if block
        if (isAuthenticated is false)
            return;

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var userId = await AuthenticationStateTask.GetUserId();

            //User = await HttpClient.User().GetUserById(userId.ToInt());
        }

        await base.OnAfterRenderAsync(firstRender);
    }
}