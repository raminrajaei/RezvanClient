using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class InstructionDocumentReadDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedAtJalali => CreatedAt.ToJalaliString();

    public int UserIdUploader { get; set; }

    public string? UserUploaderFullName { get; set; } //Flattening

    public string? FilePath { get; set; }

    public string? FileExtension { get; set; }

    public int FileType { get; set; }
    public string? FileTypeDisplay => ((Enums.Document.FileType)FileType).ToDisplayName();

    public int DocType { get; set; }
    public string? DocTypeDisplay => ((Enums.Document.InstructionDocumentType)DocType).ToDisplayName();

    public Guid Identifier { get; set; }

    public string? Description { get; set; }

    public long SizeBytes { get; set; }
}