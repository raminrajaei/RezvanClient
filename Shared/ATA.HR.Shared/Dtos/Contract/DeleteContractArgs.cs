using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class DeleteContractArgs
{
    public int ContractId { get; set; }
}