using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.CommitmentLetter;

[ComplexType]
public class DeleteCommitmentLetterArgs
{
    public int CommitmentLetterId { get; set; }
}