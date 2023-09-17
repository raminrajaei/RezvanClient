using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class RoleClaimDto
{
    [Required(ErrorMessage = "نام نقش اجباری است")]
    public string? RoleName { get; set; }

    [Required(ErrorMessage = "نوع دسترسی اجباری است")]
    public string? ClaimType { get; set; }
}