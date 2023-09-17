using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class SignatureFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? Unit { get; set; }

    public bool? HasSignature { get; set; }
}