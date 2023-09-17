using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Insurance.Models;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared;
using ATABit.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;
using Telerik.Blazor.Components;

namespace ATA.HR.Client.Web.Pages.Insurance;

[Authorize]
public partial class MyInsuredPeoplePage
{
    // Props
    public string? AvatarPicUrl { get; set; }
    private bool IsLoading { get; set; } = true;
    public OperationType PageOperationType { get; set; } = OperationType.Filter;

    private List<InsuredDto> MyInsureds { get; set; } = new();
    private UserDto CurrentUser { get; set; } = new();

    private string PersonnelCode { get; set; }
    private string BankName { get; set; } = "صادرات";
    private string CancelConfirmAlert { get; set; }
    private bool CancellingAllInsureds { get; set; }


    public bool IsVisibleCancelInsuranceConfirmDialog { get; set; }
    public InsuredDto? CancellingInsured { get; set; }

    // Index
    private TelerikGrid<InsuredDto> GridRef { get; set; }

    // Inject
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ICoreAPIs CoreAPIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    private bool DisabledDueToTimeLimitHasPassed { get; set; } = false;

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            CurrentUser = await CoreAPIs.Insurance_GetCurrentUserData();

            PersonnelCode = CurrentUser.PersonnelCode.ToString();

            AvatarPicUrl = CurrentUser.PictureURL;

            MyInsureds = await CoreAPIs.Insurance_GetMyInsureds();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<InsuredDto> obj)
    {

    }

    // Methods

    public string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{HttpClient.BaseAddress}{relativeUrl}";
    }

    public string ToDocFullURL(string filePath)
    {
        return $"{AppConstants.AppCDNBaseURL}{filePath}";
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid()
    {
        IsLoading = true;

        return GridRef.SetState(GridRef.GetState());
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        //var user = (InsuredDto)obj.Item;

        //if (user.Dismissed)
        //    obj.Class = "contract-ending-alert";
    }

    private void OnUserImageLoadFailed()
    {
        if (CurrentUser == null) return;

        AvatarPicUrl = "/images/layout/user.png";
    }

    private void OpenCancelConfirmDialog(InsuredDto insured)
    {
        if (insured.IsMainInsurer && MyInsureds.Any(i => i.IsMainInsurer == false && i.IsActive))
        {
            CancellingAllInsureds = true;
            CancelConfirmAlert = "در صورت لغو عضویت، امکان بازگشت تا پایان مدت قرارداد بیمه تکمیلی وجود ندارد. در ضمن لغو شما به منزله لغو تمامی افراد تحت تکفل شماست. آیا مطمئن به لغو عضویت می‌باشید؟";
        }
        else
        {
            CancellingAllInsureds = false;
            CancelConfirmAlert = "در صورت لغو عضویت، امکان بازگشت تا پایان مدت قرارداد بیمه تکمیلی وجود ندارد. آیا مطمئن به لغو عضویت می‌باشید؟";
        }

        IsVisibleCancelInsuranceConfirmDialog = true;

        CancellingInsured = insured;
    }

    private async Task WithdrawFromInsurance()
    {
        IsLoading = true;

        try
        {
            await CoreAPIs.Insurance_CancelInsurance(CancellingInsured.Id);

            // Rebind Grid
            MyInsureds = await CoreAPIs.Insurance_GetMyInsureds();

            // Message
            string msg = "";

            if (CancellingAllInsureds)
            {
                msg = $"لغو عضویت {CancellingInsured.FullName} و افراد تحت تکفل از بیمه تکمیلی با موفقیت انجام شد";
            }
            else
            {
                msg = $"لغو عضویت {CancellingInsured.FullName} از بیمه تکمیلی با موفقیت انجام شد";
            }

            NotificationService.Toast(NotificationType.Success, msg);
        }
        catch (Exception)
        {
            // Ignored
        }
        finally
        {
            IsLoading = false;
            IsVisibleCancelInsuranceConfirmDialog = false;
            StateHasChanged();
        }
    }

    private async Task ReviveInsuredToActiveState()
    {
        IsLoading = true;

        try
        {
            await CoreAPIs.Insurance_ReviveMainInsuredToActiveState(PersonnelCode);

            // Rebind Grid
            MyInsureds = await CoreAPIs.Insurance_GetMyInsureds();

            // Message
            string msg = "عضویت مجدد بیمه تکمیلی با موفقیت انجام شد";

            NotificationService.Toast(NotificationType.Success, msg);
        }
        catch (Exception)
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