using ATA.HR.Shared.Enums.Document;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class PersonnelDocumentDto
{
    public int? Id { get; set; }


    /// <summary>
    /// UserId the employee we are uploading document for
    /// </summary>
    public int UserId { get; set; }

    [Required(ErrorMessage = "هیچ فایلی آپلود نشده است")]
    public string? FilePath { get; set; }

    [Required(ErrorMessage = "هیچ گروه سندی انتخاب نشده است")]
    public string? DocumentCategorySelectedValue { get; set; }
    public PersonnelDocumentCategoryType? DocumentCategory => DocumentCategorySelectedValue.IsNotNullOrEmpty()
        ? (PersonnelDocumentCategoryType)DocumentCategorySelectedValue!.ToInt()
        : null;

    [Required(ErrorMessage = "هیچ زیرگروه سندی انتخاب نشده است")]
    public string? DocumentSubCategorySelectedValue { get; set; }
    public int? DocSubCategoryId => DocumentSubCategorySelectedValue.IsNotNullOrEmpty() ? DocumentSubCategorySelectedValue!.ToInt() : null;

    public Guid Identifier { get; set; }

    public long SizeBytes { get; set; }

    public string? FileExtension { get; set; }

    public string? Description { get; set; }
}