using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.Enums;

public enum OperationType
{
    Nothing,

    [Display(Name = "افزودن")]
    Add,

    [Display(Name = "فیلتر")]
    Edit,

    [Display(Name = "فیلتر")]
    Filter,

    Custom1,

    Custom2
}