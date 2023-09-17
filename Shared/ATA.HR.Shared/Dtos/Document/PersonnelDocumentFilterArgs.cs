using ATA.HR.Shared.Enums.Document;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class PersonnelDocumentFilterArgs
{
    public int UserId { get; set; }

    public string? SearchTerm { get; set; }

    public string? DocCategorySelectedValue { get; set; }
    public PersonnelDocumentCategoryType? DocumentCategory => DocCategorySelectedValue.IsNotNullOrEmpty()
        ? (PersonnelDocumentCategoryType)DocCategorySelectedValue!.ToInt()
        : null;

    public string? DocSubCategorySelectedValue { get; set; }
    public int? DocSubCategoryId => DocSubCategorySelectedValue.IsNotNullOrEmpty() ? DocSubCategorySelectedValue!.ToInt() : null;
}