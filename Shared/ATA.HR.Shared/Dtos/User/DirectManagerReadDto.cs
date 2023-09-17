using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class DirectManagerReadDto
{
    public int ConfirmerUserId { get; set; }

    public int? ConfirmerBoxId { get; set; }

    public string? ConfirmerPostTitle { get; set; }

    public string? ConfirmerFullName { get; set; }

    public int ConfirmerPersonnelCode { get; set; }

    public string? UnitName { get; set; }
}