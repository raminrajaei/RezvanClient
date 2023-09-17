using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class AllContractsFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? FlowState { get; set; }

    public string? Unit { get; set; }

    public string? WorkLocation { get; set; }

    public string? ContractProgressType { get; set; }
}