using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.WorkHours;

public enum EmployeeWorkingType
{
    [Display(Name = "ستادی")]
    Setadi = 1,

    [Display(Name = "کروی پروازی")]
    FlightCrew = 2,

    [Display(Name = "ساعتی")]
    Hourly = 3
}