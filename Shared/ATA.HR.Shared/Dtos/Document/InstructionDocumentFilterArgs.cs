using ATA.HR.Shared.Enums.Document;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class InstructionDocumentFilterArgs
{
    public string? DocTypeSelectedValue { get; set; }
    public InstructionDocumentType? InstructionDocumentType => DocTypeSelectedValue.IsNotNullOrEmpty()
        ? (InstructionDocumentType)DocTypeSelectedValue!.ToInt()
        : null;

    public string? SearchTerm { get; set; }
}