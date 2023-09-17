using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Shared.Workflow;
using Microsoft.AspNetCore.Components;
using System.Threading;
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Enums;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.Extensions;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Microsoft.AspNetCore.Components.Authorization;

namespace ATA.HR.Client.Web.Pages.Dashboard;

public partial class InsuranceFlowFormsPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private InsuranceFlowFormsDto InsuranceFlowForms { get; set; }
    private DoActionArgs Action { get; set; } = new();
    private WorkFullHistory History { get; set; }
    private PossibleActions PossibleActions { get; set; }
    private int? ToDoPersonnelCode { get; set; }
    private ConfirmCostCommand ConfirmCostCommand { get; set; } = new();
    private bool ViewFormByBozorgi { get; set; } = true;
    private bool IsVisibleEditAmountAndObligationTypeModal { get; set; }
    private List<SelectListItem> ObligationTypesSource { get; set; } = new();
    private UpdateCostAmountAndObligationTypeCommand UpdateCostAmountAndObligationTypeCommand { get; set; } = new();
    private GetRemainedBalanceQuery GetRemainedBalanceQuery { get; set; } = new();
    private int RemainedBalance { get; set; }
    private BankPaymentDto? BankPayment { get; set; }

    private bool IsCostEditable => InsuranceFlowForms.IsFlowFinished is false
                                   && InsuranceFlowForms.Cost != null
                                   && InsuranceFlowForms.IsCurrentUserActor
                                   && (InsuranceFlowForms.Cost.FlowCurrentStateTag!.StartsWith("HRCheck")
                                       || InsuranceFlowForms.Cost.FlowCurrentStateTag == InsuranceStateTag.ClaimsAdjusterAppraisal.ToString());

            // Parameters
    [Parameter] public int CostId { get; set; }
    [Parameter] public int? FromAllCostsPage { get; set; }

    // Injects
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private ICoreAPIs CoreAPIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            var userId = await AuthenticationStateTask.GetUserId();

            ViewFormByBozorgi = userId is "1807" or "27";

            await InitializePage();

            await InitializeFlow();
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

        await base.OnInitializedAsync(cancellationToken);
    }

    private async Task InitializeFlow()
    {
        History = await CoreAPIs.Insurance_GetCostFlowHistory(CostId);

        ToDoPersonnelCode = History.ToDoes?.Users?.FirstOrDefault()?.PersonnelCode;

        var obligations = await CoreAPIs.Insurance_GetActivePolicyObligations();

        ObligationTypesSource = obligations.Select(o => new SelectListItem($"{o.ObligationCode} – {o.ObligationTitle}", o.Id.ToString())).ToList();

        if (InsuranceFlowForms.IsCurrentUserActor)
        {
            PossibleActions = await CoreAPIs.Insurance_GetPossibleActions(CostId);
        }
    }

    private async Task InitializePage()
    {
        InsuranceFlowForms = await CoreAPIs.Insurance_GetCostFlowForms(CostId);

        ConfirmCostCommand.CostId = CostId;
        ConfirmCostCommand.ConfirmedAmount = InsuranceFlowForms.Cost!.ConfirmedAmount;
        ConfirmCostCommand.FranchisePercent = InsuranceFlowForms.Cost.FranchisePercent.HasValue
            ? InsuranceFlowForms.Cost.FranchisePercent.Value
            : 10;

        UpdateCostAmountAndObligationTypeCommand.CostId = CostId;
        UpdateCostAmountAndObligationTypeCommand.PolicyObligationId = InsuranceFlowForms.Cost.PolicyObligationId;
        UpdateCostAmountAndObligationTypeCommand.PaidAmount = InsuranceFlowForms.Cost.PaidAmount;

        GetRemainedBalanceQuery.CostId = CostId;
        GetRemainedBalanceQuery.InsuredId = InsuranceFlowForms.Cost.InsuredId;
        GetRemainedBalanceQuery.PolicyObligationId = InsuranceFlowForms.Cost.PolicyObligationId;

        await UpdateTheRemainedBalance();

        if (InsuranceFlowForms.Cost.BankPaymentId.HasValue)
        {
            var allPayments = await CoreAPIs.Insurance_GetAllBankPayments();

            BankPayment = allPayments.FirstOrDefault(p => p.Id == InsuranceFlowForms.Cost.BankPaymentId.Value);
        }
    }

    private void NavigateBackToIncomingPage()
    {
        if(FromAllCostsPage == 1)
            NavigationManager.NavigateTo(PageUrls.AllCostsPage);
        else
            NavigationManager.NavigateTo(PageUrls.MyCostsPage);
    }

    //  Methods
    private async Task DoAction(string actionTag)
    {
        try
        {
            if (InsuranceFlowForms.Cost?.FlowCurrentStateTag == InsuranceStateTag.CostEdit.ToString())
            {
                if (string.IsNullOrWhiteSpace(Action.Comment))
                {
                    NotificationService.Toast(NotificationType.Error, "نوشتن توضیحات اقدامات انجام گرفته برای رفع نواقص الزامی می‌باشد");

                    return;
                }
            }

            IsLoading = true;

            var actionArg = new DoActionArgs
            {
                ActionTag = actionTag,
                Key = CostId,
                Comment = Action.Comment
            };

            await CoreAPIs.Insurance_DoActionOnCost(actionArg);

            NotificationService.Toast(NotificationType.Success, "عملیات با موفقیت انجام شد");

            await InitializePage();

            await InitializeFlow();

            StateHasChanged();
        }
        catch
        {
            // ignored
        }
        finally
        {
            StateHasChanged();

            IsLoading = false;
        }
    }

    private Task OkAction() => DoAction(ActionType.Confirm.ToString());

    private async Task OnConfirmCost()
    {
        if (ValidateCostConfirmOnSubmit() is false) return;

        IsLoading = true;

        try
        {
            await CoreAPIs.Insurance_ConfirmCost(ConfirmCostCommand);

            await UpdateTheRemainedBalance();

            NotificationService.Toast(NotificationType.Success, "هزینه با موفقیت تایید شد. برای بستن گردش کار و نهایی کردن این هزینه، دکمه‌ی تایید را بزنید");
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

    private bool ValidateCostConfirmOnSubmit()
    {
        if (ConfirmCostCommand.ConfirmedAmount.HasValue is false)
        {
            NotificationService.Toast(NotificationType.Error, "مبلغ تایید شده را وارد نمایید");

            return false;
        }

        //if (FranchisePercent.HasValue is false)
        //{
        //    NotificationService.Toast(NotificationType.Error, "درصد فرانشیز را وارد نمایید");

        //    return false;
        //}

        return true;
    }

    private void GoBackToNormalModeFromUpdateMode()
    {
        IsVisibleEditAmountAndObligationTypeModal = false;
    }

    private void OpenUpdateCostModal()
    {
        IsVisibleEditAmountAndObligationTypeModal = true;
    }

    private async Task EditCost()
    {
        IsLoading = true;

        try
        {
            await CoreAPIs.Insurance_UpdateCostAmountAndObligationType(UpdateCostAmountAndObligationTypeCommand);

            NotificationService.Toast(NotificationType.Success, "بروزرسانی با موفقیت انجام شد");

            await InitializePage();
        }
        catch
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            IsVisibleEditAmountAndObligationTypeModal = false;

            StateHasChanged();
        }
    }

    private async Task UpdateTheRemainedBalance()
    {
        RemainedBalance = await CoreAPIs.Insurance_GetRemainedBalance(GetRemainedBalanceQuery);
    }
}