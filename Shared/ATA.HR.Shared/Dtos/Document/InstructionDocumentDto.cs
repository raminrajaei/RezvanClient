using ATA.HR.Shared.Enums.Document;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class InstructionDocumentDto
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "هیچ فایلی آپلود نشده است")]
    public string? FilePath { get; set; }

    [Required(ErrorMessage = "هیچ گروه سندی انتخاب نشده است")]
    public string? DocTypeSelectedValue { get; set; }
    public InstructionDocumentType? DocumentType => DocTypeSelectedValue.IsNotNullOrEmpty()
        ? (InstructionDocumentType)DocTypeSelectedValue!.ToInt()
        : null;

    public Guid Identifier { get; set; }

    public long SizeBytes { get; set; }

    public string? FileExtension { get; set; }

    public string? Description { get; set; }
}