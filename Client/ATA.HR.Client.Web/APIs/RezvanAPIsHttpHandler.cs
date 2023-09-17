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
using ATA.Core.Modules.Model.Error;
using ATA.HR.Client.Web.APIs.Models.Response;
using BootstrapBlazor.Components;
using Console = System.Console;

namespace ATA.HR.Client.Web.APIs;

public class RezvanAPIsHttpHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;
    private readonly INotificationService _notificationService;
    private readonly NavigationManager _navigationManager;
    private readonly ClientAppSettings _clientAppSettings;

    #region Constructor Injections

    public RezvanAPIsHttpHandler(ILocalStorageService localStorageService, INotificationService notificationService, NavigationManager navigationManager, ClientAppSettings clientAppSettings)
    {
        _localStorageService = localStorageService;
        _notificationService = notificationService;
        _navigationManager = navigationManager;
        _clientAppSettings = clientAppSettings;
    }

    #endregion

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        #region Add Auth header to request - DESABLED FOR REZVAN PROJECT

        if (false && request.Headers.Contains(AppConstants.AuthToken.Scheme) is false)
        {
            var token = await _localStorageService.GetItemAsStringAsync(AppConstants.AuthToken.JwtTokenLocalStorageKey);

            if (token.IsNotNullOrEmpty())
                request.Headers.Authorization = new AuthenticationHeaderValue(AppConstants.AuthToken.Scheme, token);
        }

        #endregion

        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode is false && response.StatusCode != HttpStatusCode.BadRequest)
        {
            var exceptionResultString = await response.Content.ReadAsStringAsync(cancellationToken);

            Console.WriteLine(exceptionResultString);

            await _notificationService.AlertAsync(NotificationType.Error, "متاسفانه خطایی روی داده است. دوباره امتحان کنید و در صورت عدم حل مشکل، با پشتیبانی تماس بگیرید");

            // REZVAN PROJECT DOESN'T COMPLY WITH THROW ERROR RULE WITH ErrorModel

            //var exceptionResult = exceptionResultString.DeserializeToModel<ErrorModel>();

            //var message = $"{exceptionResult.Message}";

            //switch (response.StatusCode)
            //{
            //    case HttpStatusCode.NotFound:
            //        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "Not found" : message);
            //        break;

            //    case HttpStatusCode.Forbidden:
            //        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "You don't have permission" : message);
            //        break;

            //    case HttpStatusCode.UnprocessableEntity:
            //        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "عملیات با خطا همراه شد" : message);
            //        break;

            //    case HttpStatusCode.Unauthorized:
            //        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "You should login" : message);
            //        await Task.Delay(2000, cancellationToken);
            //        _navigationManager.NavigateTo(_ssoClient.GetSSOLoginPageUrl(_clientAppSettings.UrlSettings!.AppURL!));
            //        break;

            //    default:
            //        await _notificationService.AlertAsync(NotificationType.Error, string.IsNullOrWhiteSpace(message) ? "Some error happened" : message);
            //        break;
            //}
        }

        // TODO: HANDLE THE ERROR IN ApiResult MODEL SENT BY REZVAN APIs

        var rawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var api = rawResponse.DeserializeToModel<ApiResult>();

        if (api.IsSuccess is false)
        {
            await _notificationService.AlertAsync(NotificationType.Error, api.Message);
        }

        return response;
    }
}