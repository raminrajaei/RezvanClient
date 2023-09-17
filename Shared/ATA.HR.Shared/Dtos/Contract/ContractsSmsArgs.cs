using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class ContractsSmsArgs
{
    public AllContractsFilterArgs ContractsFilterArgs { get; set; } = new();

    [Required(ErrorMessage = "متن پیامک را وارد نمایید")]
    public string? SmsContent { get; set; }
}