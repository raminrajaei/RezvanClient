using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Workflow;

public enum FlowStatus
{
    [Display(Name = "در حال بررسی")]
    Pending = 0,

    [Display(Name = "تایید شده")]
    Succeeded = 1,

    //[Display(Name = "رد شده")]
    //Rejected = 2
}