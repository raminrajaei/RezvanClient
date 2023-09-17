using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Pages.Dashboard.Models;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Dtos.Workflow;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Shared.Workflow;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Pages.Dashboard;

public partial class ContractFlowFormsPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    public int ContractVersion { get; set; }
    public AllContractFormsReadDto AllContractForms { get; set; }
    private DoActionArgs Action { get; set; } = new();
    public WorkFullHistory History { get; set; }
    public PossibleActions PossibleActions { get; set; }
    public bool HasActiveSignature { get; set; }
    public int? ToDoPersonnelCode { get; set; }
    public bool IsVisibleSMSConfirmationCodeBox { get; set; }
    public string? SmsConfirmationCode { get; set; }
    public bool IsVisibleContractDeleteButton { get; set; }
    public bool IsVisibleEditContractDatesButton { get; set; }
    public bool IsPreviewPage { get; set; }
    public bool IsHourlyContract { get; set; }
    public bool IsVisibleDeleteContractConfirmDialog { get; set; }
    public bool IsVisibleEditContractStartDateWindow { get; set; }
    public bool IsVisibleEditContractEndDateWindow { get; set; }
    public int DeletingOrEditingContractId { get; set; }
    public string? DeletingContractPersonnelName { get; set; }
    public string? NewStartDateWhileEditing { get; set; }
    public string? NewEndDateWhileEditing { get; set; }
    public string? UserMobile { get; set; }
    public bool HasPrintPermission { get; set; }

    // Parameters
    [Parameter] public int ContractId { get; set; }
    [Parameter] public string PageType { get; set; }

    // Cascading Parameters
    [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Injects
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private INotificationService NotificationService { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        if (PageType == "preview")
            IsPreviewPage = true;

        HasPrintPermission = await AuthenticationStateTask.HasUserClaim(Claims.Pages_Contracts_StartNewContractFlow);

        try
        {
            IsHourlyContract = await HttpClient.Contract().IsHourlyContract(ContractId, cancellationToken: cancellationToken);

            if (IsPreviewPage is false)
            {
                AllContractForms = await HttpClient.FlowForm().GetAllContractForms(ContractId, cancellationToken: cancellationToken);

                ContractVersion = AllContractForms.Contract.ContractVersion!.Value;

                History = await HttpClient.FlowForm().GetContractHistory(ContractId, cancellationToken: cancellationToken);

                ToDoPersonnelCode = History.ToDoes?.Users?.FirstOrDefault()?.PersonnelCode;

                HasActiveSignature = await HttpClient.User().HasCurrentUserActiveSignature(cancellationToken: cancellationToken);

                if (AllContractForms.IsCurrentUserActor)
                {
                    if (HasActiveSignature is false)
                    {
                        NotificationService.Toast(NotificationType.Warning,
                            "امضای شما در سیستم تعریف نشده است. لطفا ابتدا امضای خود را در سیستم آتا ثبت نموده و سپس این قرارداد را بررسی و تایید نمایید");
                    }

                    PossibleActions = await HttpClient.Action()
                        .UserPossibleActions((int)ClientFlowType.EmployeeContract, ContractId, cancellationToken: cancellationToken);
                }

                IsVisibleContractDeleteButton = AllContractForms.IsFlowFinished is false;

                IsVisibleEditContractDatesButton = true; //AllContractForms.HasFlowJustStarted;

                NewStartDateWhileEditing = AllContractForms.Contract.ContractDetailsExecutionDateJalaliDisplay;

                NewEndDateWhileEditing = AllContractForms.Contract.ContractDetailsValidityDateJalaliDisplay;
            }
            else //Is Preview
            {
                ContractVersion = await HttpClient.Contract().GetContractVersion(ContractId, cancellationToken: cancellationToken);
            }
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

    private void NavigateBackToDashboardPage()
    {
        NavigationManager.NavigateTo(PageUrls.Dashboard);
    }

    //  Methods
    private async Task DoAction(string actionTag)
    {
        try
        {
            IsLoading = true;

            var actionArg = new DoActionArgs
            {
                ActionTag = actionTag,
                Key = ContractId,
                Comment = Action.Comment
            };

            var actionResponse = await HttpClient.Action().DoActionOnContract(new DoActionOnContractArgs
            {
                DoAction = actionArg,
                ConfirmationCode = SmsConfirmationCode
            });

            if (actionResponse.OpenTheSMSBoxToEnterCode)
            {
                IsVisibleSMSConfirmationCodeBox = true;

                UserMobile = actionResponse.UserMobile;

                NotificationService.Toast(NotificationType.Success,
                    actionResponse.IsUsingNationalCodeInsteadOfSmsCode
                        ? "همکار گرامی، کد ملی خود را در باکس تایید وارد نمایید"
                        : $"کد تایید به همراه شما به شماره‌ی {actionResponse.UserMobile} ارسال شد. این کد به مدت {actionResponse.SmsCodeIsValidForSeconds} ثانیه معتبر می‌باشد");
            }
            else
            {
                NotificationService.Toast(NotificationType.Success, "عملیات با موفقیت انجام شد");

                IsLoading = false;

                StateHasChanged();

                await Task.Delay(2000);

                NavigationManager.NavigateTo(PageUrls.ContractFlowFormsPage(ContractId), true);
            }
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

    private async Task ConfirmActionWithSMSCode()
    {
        if (string.IsNullOrWhiteSpace(SmsConfirmationCode))
        {
            NotificationService.Toast(NotificationType.Error, "کد تایید را وارد نمایید");

            return;
        }

        await DoAction(ActionType.Confirm.ToString());
    }

    private async Task DeleteContract(int contractId)
    {
        DeletingOrEditingContractId = contractId;

        var contract = await HttpClient.Contract().GetContractById(contractId);

        DeletingContractPersonnelName = contract.ContractDetailsFullName;

        IsVisibleDeleteContractConfirmDialog = true;
    }

    private async Task DeleteContractById(int contractId)
    {
        IsLoading = true;

        try
        {
            await HttpClient.Contract().DeleteContract(new DeleteContractArgs { ContractId = contractId });

            NotificationService.Toast(NotificationType.Success, "حذف قرارداد با موفقیت انجام شد");

            IsLoading = false;

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

    private async Task ChangeContractStartDate()
    {
        IsLoading = true;

        try
        {
            await HttpClient.Contract().EditContractStartDate(new EditContractDateArgs
            {
                ContractId = DeletingOrEditingContractId,
                NewJalaliDate = NewStartDateWhileEditing
            });

            IsVisibleEditContractStartDateWindow = false;

            NotificationService.Toast(NotificationType.Success, "تغییر تاریخ شروع با موفقیت انجام شد. برای مشاهده تغییرات صفحه را رفرش کنید");
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private void OpenEditContractStartDateModal(int contractId)
    {
        DeletingOrEditingContractId = contractId;
        IsVisibleEditContractStartDateWindow = true;
    }

    private void OpenEditContractEndDateModal(int contractId)
    {
        DeletingOrEditingContractId = contractId;
        IsVisibleEditContractEndDateWindow = true;
    }

    private async Task ChangeContractEndDate()
    {
        IsLoading = true;

        try
        {
            await HttpClient.Contract().EditContractEndDate(new EditContractDateArgs
            {
                ContractId = DeletingOrEditingContractId,
                NewJalaliDate = NewEndDateWhileEditing
            });

            IsVisibleEditContractEndDateWindow = false;

            NotificationService.Toast(NotificationType.Success, "تغییر تاریخ پایان قرارداد با موفقیت انجام شد. برای مشاهده تغییرات صفحه را رفرش کنید");
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}