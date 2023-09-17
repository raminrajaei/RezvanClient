using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos.Contract;
using ATABit.Shared;
using Bit.Http.Contracts;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Contract;

public partial class DismissSmsesPage
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    public bool ResetPagination { get; set; }
    public bool IsViewingStillHaveNotBeenSentGrid { get; set; } = true;
    public int UserIdToSendSmsTo { get; set; }
    public string? SmsContentToSend { get; set; }
    public string? UserFullNameSendingSmsTo { get; set; }
    public bool IsVisibleSendingSmsModal { get; set; }

    // Index
    private TelerikGrid<UserDto> HaveNotSentUsersGridRef { get; set; }
    private TelerikGrid<DismissSmsReadDto> HaveSentUsersGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
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

        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<UserDto> obj)
    {

    }

    private void OnStateInitHandler2(GridStateEventArgs<DismissSmsReadDto> obj)
    {

    }

    // Method
    protected async Task OnReadHandlerHaveNotSentUsersGrid(GridReadEventArgs args)
    {
        await LoadHaveNotSentUsersData(args);
    }

    protected async Task OnReadHandlerHaveSentUsersGrid(GridReadEventArgs args)
    {
        await LoadHaveSentUsersData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private async Task RebindGrids(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        //IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;


        await HaveNotSentUsersGridRef.SetState(HaveNotSentUsersGridRef.GetState());

        if (HaveSentUsersGridRef != null)
            await HaveSentUsersGridRef.SetState(HaveSentUsersGridRef.GetState());
    }

    public async Task LoadHaveNotSentUsersData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                HaveNotSentUsersGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = CancellationToken.None;

            args.Data = await HttpClient.User().GetPersonnelDismissedButHaveNotSentSmsYet(context, cancellationToken);

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            //IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public async Task LoadHaveSentUsersData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                HaveSentUsersGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = CancellationToken.None;

            args.Data = await HttpClient.User().GetPersonnelDismissedAndSmsHasBeenSentToThem(context, cancellationToken);

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            //IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public Task ApplyFilters => RebindGrids(true);

    //private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        //var workHour = (WorkHourReadDto)obj.Item;

        //if (workHour.LastContractEndDate is not null && workHour.LastContractEndDate.Value - DateTime.Now < TimeSpan.FromDays(30))
        //    obj.Class = "contract-ending-alert";
    }

    private void OnGridRowRender2(GridRowRenderEventArgs obj)
    {
        //var workHour = (WorkHourReadDto)obj.Item;

        //if (workHour.LastContractEndDate is not null && workHour.LastContractEndDate.Value - DateTime.Now < TimeSpan.FromDays(30))
        //    obj.Class = "contract-ending-alert";
    }

    private void ChangeToViewingStillHaveNotBeenSentGrid()
    {
        IsViewingStillHaveNotBeenSentGrid = true;
    }

    private void ChangeToViewingHaveBeenSentGrid()
    {
        IsViewingStillHaveNotBeenSentGrid = false;
    }

    private async Task OnSendDismissSms()
    {
        // RoutePlanning Logic. I didn't grasp it so I commented it out
        //if (PageOperationType is OperationType.Edit && IsSaveAndContinue)
        //    WorkHour.Id = null;

        if (UserIdToSendSmsTo == default || string.IsNullOrWhiteSpace(UserFullNameSendingSmsTo) || string.IsNullOrWhiteSpace(SmsContentToSend))
        {
            NotificationService.Toast(NotificationType.Error, "خطا در عملیات، متن پیامک نمی‌تواند خالی باشد");

            return;
        }

        IsLoading = true;

        try
        {
            var msg = "";

            await HttpClient.User().SendDismissSmsToUser(UserIdToSendSmsTo, SmsContentToSend);

            IsVisibleSendingSmsModal = false;

            msg = $"پیامک خاتمه خدمت {UserFullNameSendingSmsTo} با موفقیت ارسال شد";

            NotificationService.Toast(NotificationType.Success, msg);

            await RebindGrids(false);
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

    private void ShowSendingSmsModal(int userId, int? gender, string fullName)
    {
        UserIdToSendSmsTo = userId;

        UserFullNameSendingSmsTo = fullName;

        var mrOrMrs = gender == 1 ? "آقای" : "خانم";
        SmsContentToSend = $"همکار گرامی {mrOrMrs} {fullName} با توجه به اتمام قرارداد جنابعالی، خواهشمند است در اسرع وقت با مراجعه به معاونت منابع انسانی آتا نسبت به انجام تسویه حساب اقدام بفرمایید";

        IsVisibleSendingSmsModal = true;
    }
}

