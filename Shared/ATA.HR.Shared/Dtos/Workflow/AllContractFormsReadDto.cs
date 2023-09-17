using ATA.HR.Shared.Dtos.Contract;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public class AllContractFormsReadDto
{
    public bool IsCurrentUserActor { get; set; }

    public bool IsFlowFinished { get; set; }

    public bool HasFlowJustStarted { get; set; }

    public ContractReadDto Contract { get; set; } = new();
}