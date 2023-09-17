using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class RoleReadDto
{
    public string? RoleName { get; set; }

    public List<string> Claims { get; set; } = new();
}