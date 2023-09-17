using ATA.HR.Shared.Enums.Document;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class PersonnelDocumentMiniReadDto
{
    public int DocCategory { get; set; }
    public string? DocCategoryDisplay => ((PersonnelDocumentCategoryType)DocCategory).ToDisplayName();

    public int SubCategoryId { get; set; }

    public string? SubCategoryTitle { get; set; } //Flattening
}