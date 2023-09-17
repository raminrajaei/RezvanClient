using ATA.HR.Shared.Enums.WorkHours;
using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours;

[ComplexType]
public class WorkHoursFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? EmployeeWorkingTypeSelectedValue { get; set; } = EmployeeWorkingType.Setadi.ToString("D");
    public EmployeeWorkingType? EmployeeWorkingTypeFilter => EmployeeWorkingTypeSelectedValue.IsNotNullOrEmpty()
        ? (EmployeeWorkingType)EmployeeWorkingTypeSelectedValue!.ToInt()
        : null;

    public string? YearSelectedValue { get; set; } = DateTime.Now.GetPersianYear().ToString();

    public string? MonthSelectedValue { get; set; } = DateTime.Now.GetPersianMonth().ToString();

    public string? UnitName { get; set; }

    public string? WorkLocation { get; set; }

    public string? RegistrarUserIdSelectedValue { get; set; }

    public string? FlowCurrentStateTag { get; set; }

    public bool OnlyIsChanged { get; set; }

    public bool OnlyWithAbsentDay { get; set; }
}