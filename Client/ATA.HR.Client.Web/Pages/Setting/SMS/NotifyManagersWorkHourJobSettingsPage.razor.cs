using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos.AppGeneric.DbAppSettings;
using ATA.HR.Shared.Dtos.CommitmentLetter;
using ATABit.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading;
using Telerik.Blazor.Components;

namespace ATA.HR.Client.Web.Pages.Setting.SMS;

[Authorize]
public partial class NotifyManagersWorkHourJobSettingsPage
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    public OperationType PageOperationType { get; set; } = OperationType.Edit;
    public NotifyDirectManagersToConfirmWorkHoursDbSettings JobDbSettings { get; set; } = new();
    public string? JobEnabledSelectedValue { get; set; }
    public List<SelectListItem> ExcludedUsers { get; set; } = new();

    private List<SelectListItem> JobStatusSource { get; set; } = new()
    {
        new SelectListItem("فعال", "1"),
        new SelectListItem("غیرفعال", "0")
    };

    private const string JobDescription = @"توضیح: ابتدا حدود یکساعت پس از تایید مدیر منابع انسانی اولین پیامک به مدیر 
        ارسال می‌شود. سپس در صورت عدم تایید مدیر، حدود ساعت 10 صبح روز بعد پیامک بعدی ارسال می‌شود. در آخر
        و در صورت عدم تایید، ساعت 10 صبح بعد سومین پیامک نیز به مدیر ارسال می‌شود. در صورت عدم توجه
        مدیر، حدود 2 الی 3 ساعت بعد از آخرین پیامک، به پشتیبانی پیامی مبنی بر عدم توجه مدیر ارسال می‌شود.";

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            JobDbSettings = await HttpClient.JobSettings().GetNotifyManagersWorkHourJobSettings(cancellationToken: cancellationToken);

            JobEnabledSelectedValue = JobDbSettings!.IsEnabled ? "1" : "0";

            ExcludedUsers = await HttpClient.JobSettings().GetExcludedUsersFromNotifyManagersWorkHourJobSettings(cancellationToken: cancellationToken);
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    // Methods

    public string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{HttpClient.BaseAddress}{relativeUrl}";
    }



    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var commitmentLetter = (CommitmentLetterReadDto)obj.Item;

        if (commitmentLetter.ValidityDuration < TimeSpan.FromDays(60))
            obj.Class = "contract-ending-alert";
    }


    private async Task OnSaveSettings()
    {
        IsLoading = true;

        try
        {
            JobDbSettings.IsEnabled = JobEnabledSelectedValue == "1";

            await HttpClient.JobSettings().UpdateNotifyManagersWorkHourJobSettings(JobDbSettings);

            NotificationService.Toast(NotificationType.Success, "تنظیمات با موفقیت ذخیره شد");
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

    private async Task DeleteUserFromExcludes(int userId)
    {

    }

    private async Task DeleteUserFromSupport(string mobile)
    {
        throw new NotImplementedException();
    }
}