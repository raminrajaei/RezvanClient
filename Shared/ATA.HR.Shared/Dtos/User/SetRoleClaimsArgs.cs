using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class SetRoleClaimsArgs
{
    [Required(ErrorMessage = "نقش اجباری است")]
    public string? RoleName { get; set; }

    public List<string> NewClaims { get; set; } = new();
}