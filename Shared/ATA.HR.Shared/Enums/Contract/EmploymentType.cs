using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Contract;

// Map Rahkaran EmploymentID to Employment Status
public enum EmploymentType
{
    [Display(Name = "قراردادی")]
    Contractual = 1,

    [Display(Name = "قراردادی – بازنشسته")]
    ContractualRetired = 2,

    [Display(Name = "مشاوره‌ای")]
    Consulting = 3,

    [Display(Name = "ساعتی")]
    Hourly = 4
}