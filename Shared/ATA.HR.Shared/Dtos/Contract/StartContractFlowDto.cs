using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class StartContractFlowDto
{
    public int UserId { get; set; }

    public Guid? BatchIdentifier { get; set; }
}