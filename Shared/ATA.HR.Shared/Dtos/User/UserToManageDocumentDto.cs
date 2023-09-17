using ATA.HR.Shared.Dtos.Document;
using ATABit.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UserToManageDocumentDto : UserDto
{
    public int DocumentPersonnelCount { get; set; } //Flattening

    public string? PictureURLToDisplay { get; set; }

    public List<PersonnelDocumentMiniReadDto> DocumentPersonnel { get; set; } = new();
    public List<string> UserDocSubCategories => DocumentPersonnel.Select(d => d.SubCategoryTitle).Distinct().ToList();
    public List<string> UserDocCategories => DocumentPersonnel.Select(d => d.DocCategoryDisplay).Distinct().ToList();
}