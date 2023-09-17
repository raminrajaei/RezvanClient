using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class WorkHourToBeSentDto
{
    public string? FullName { get; set; }

    public int UserId { get; set; }

    public int WorkHourId { get; set; }
}