using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Workflow;

public enum DashboardType
{
    [Display(Name = "قرارداد")]
    Contracts = 0,

    [Display(Name = "کارکرد")]
    WorkHours = 1
}