using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UserSignatureDto
{
    [Required]
    public string? SignatureData { get; set; }

    [Required]
    public int PersonnelCode { get; set; }
}