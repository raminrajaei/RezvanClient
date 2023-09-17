using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Pages.Dashboard.Models;
using ATA.HR.Shared.Dtos.Workflow;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Shared.Workflow;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Pages.Dashboard;

public partial class WorkHoursFlowFormsPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    public AllWorkHourFormsReadDto AllWorkHoursForms { get; set; }
    private DoActionArgs Action { get; set; } = new();
    public WorkFullHistory History { get; set; }
    public PossibleActions PossibleActions { get; set; }
    public bool HasActiveSignature { get; set; }
    public int? ToDoPersonnelCode { get; set; }

    // Parameters
    [Parameter] public int WorkHoursId { get; set; }

    // Injects
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private INotificationService NotificationService { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            AllWorkHoursForms = await HttpClient.FlowForm().GetAllWorkHourForms(WorkHoursId, cancellationToken: cancellationToken);

            History = await HttpClient.FlowForm().GetWorkHourHistory(WorkHoursId, cancellationToken: cancellationToken);

            ToDoPersonnelCode = History.ToDoes?.Users?.FirstOrDefault()?.PersonnelCode;

            HasActiveSignature = await HttpClient.User().HasCurrentUserActiveSignature(cancellationToken: cancellationToken);

            if (AllWorkHoursForms.IsCurrentUserActor)
            {
                if (HasActiveSignature is false)
                {
                    NotificationService.Toast(NotificationType.Warning,
                        "امضای شما در سیستم تعریف نشده است. لطفا ابتدا امضای خود را در سیستم آتا ثبت نموده و سپس این کارکرد را بررسی و تایید نمایید");
                }

                PossibleActions = await HttpClient.Action()
                    .UserPossibleActions((int)ClientFlowType.EmployeeWorkHours, WorkHoursId, cancellationToken: cancellationToken);
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
                Key = WorkHoursId,
                Comment = Action.Comment
            };

            await HttpClient.Action().DoActionOnWorkHour(actionArg);

            NotificationService.Toast(NotificationType.Success, "عملیات با موفقیت انجام شد");

            IsLoading = false;

            StateHasChanged();

            await Task.Delay(2000);

            NavigationManager.NavigateTo(PageUrls.WorkHoursFlowFormsPage(WorkHoursId), true);
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
}