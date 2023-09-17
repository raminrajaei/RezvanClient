using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.CommitmentLetter;

[ComplexType]
public class DeleteWorkHourArgs
{
    public int WorkHourId { get; set; }
}