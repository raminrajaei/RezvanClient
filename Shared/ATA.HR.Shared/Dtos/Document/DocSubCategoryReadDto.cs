using ATA.HR.Shared.Enums.Document;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class DocSubCategoryReadDto
{
    public int Id { get; set; }

    public int DocumentCategoryValue { get; set; }
    public PersonnelDocumentCategoryType PersonnelDocumentCategoryType => (PersonnelDocumentCategoryType)DocumentCategoryValue;

    public string? Title { get; set; }


}