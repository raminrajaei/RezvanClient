using ATA.HR.Shared.Enums.Workflow;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public class DashboardFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? FlowState { get; set; }

    public string? DashboardTypeSelectedValue { get; set; } = DashboardType.Contracts.ToString("D");

    public string? Unit { get; set; }

    public bool IsToDo { get; set; } = true; //From WorkList

    public string? ContractYear { get; set; }

    public string? WorkHourYearSelectedValue { get; set; }

    public string? WorkHourMonthSelectedValue { get; set; }
}