using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class EditContractDateArgs
{
    public int ContractId { get; set; }

    public string? NewJalaliDate { get; set; }
}