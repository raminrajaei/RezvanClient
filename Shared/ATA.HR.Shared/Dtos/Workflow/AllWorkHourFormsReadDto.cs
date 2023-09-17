using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public class AllWorkHourFormsReadDto
{
    public bool IsCurrentUserActor { get; set; }

    public bool IsFlowFinished { get; set; }

    public bool HasFlowJustStarted { get; set; }

    //public WorkHourReadDto WorkHour { get; set; } = new();
}