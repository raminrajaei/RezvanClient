using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class ContractPreviewDto
{
    public int UserId { get; set; }
}