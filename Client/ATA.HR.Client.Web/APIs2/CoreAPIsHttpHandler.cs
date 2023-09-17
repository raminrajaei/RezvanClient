using ATA.Broker.SSOSecurity.Contract;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models.AppSettings;
using ATA.HR.Shared;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace ATA.HR.Client.Web.APIs;

public class CoreAPIsHttpHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly INotificationService _notificationService;
    private readonly NavigationManager _navigationManager;
    private readonly IATASSOClientService _ssoClient;
    private readonly ClientAppSettings _clientAppSettings;

    #region Constructor Injections

    public CoreAPIsHttpHandler(ILocalStorageService localStorageService, INotificationService notificationService, NavigationManager navigationManager, IATASSOClientService ssoClient, ClientAppSettings clientAppSettings)
    {
        _localStorageService = localStorageService;
        _notificationService = notificationService;
        _navigationManager = navigationManager;
        _ssoClient = ssoClient;
        _clientAppSettings = clientAppSettings;
    }

    #endregion

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        #region Add Auth header to request

        if (request.Headers.Contains(AppConstants.AuthToken.Scheme) is false)
        {
            var token = await _localStorageService.GetItemAsStringAsync(AppConstants.AuthToken.JwtTokenLocalStorageKey);

            if (token.IsNotNullOrEmpty())
                request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.AuthToken.Scheme, token);
        }

        #endregion

        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}