using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class DeleteClaimFromRoleArgs
{
    [Required(ErrorMessage = "شناسه اجباری است")]
    public int? RoleClaimId { get; set; }
}