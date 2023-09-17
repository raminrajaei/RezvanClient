using ATABit.Shared.Workflow;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public class DoActionOnContractArgs
{
    public DoActionArgs DoAction { get; set; } = new();

    public string? ConfirmationCode { get; set; }
}