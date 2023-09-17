using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours;

[ComplexType]
public class WorkHoursCountResult
{
    public int SetadiCount { get; set; }

    public int FlightCrewCount { get; set; }

    public int HourlyCount { get; set; }
}