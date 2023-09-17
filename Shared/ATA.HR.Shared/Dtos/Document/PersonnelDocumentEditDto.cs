using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public record PersonnelDocumentEditDto(int DocumentId, string? Description);