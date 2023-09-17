using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours;

[ComplexType]
public class WorkHourFlowDirectManagerCommentDto
{
    public int Year { get; set; }

    public int Month { get; set; }

    public string? Comment { get; set; }
}