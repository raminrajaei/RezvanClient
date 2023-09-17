using ATA.HR.Shared.Dtos.WorkHours;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Components;

public partial class WorkHours
{
    [Parameter] public int WorkHoursId { get; set; }

    [Parameter] public bool HasCurrentUserActiveSignature { get; set; }

    [Parameter] public bool IsCurrentUserActor { get; set; }

    public string DisabledStyle => false && HasCurrentUserActiveSignature is false && IsCurrentUserActor ? "opacity: 0.3" : "";

    [Parameter] public int? ToDoPersonnelCode { get; set; } //The personnel which should confirm. Is used for Signature show

    public WorkHourReadDto? UserWorkHour { get; set; }

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        UserWorkHour = await HttpClient.WorkHour().GetWorkHourById(WorkHoursId, cancellationToken: cancellationToken);

        await base.OnInitializedAsync(cancellationToken);
    }
}