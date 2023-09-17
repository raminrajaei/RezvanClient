using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Contract;

public enum ContractProgressType
{
    [Display(Name = "در انتظار امضای پرسنل")]
    WaitingForEmployeeSignature = 0,

    //[Display(Name = "امضای خود پرسنل انجام شده")]
    //SignedByEmployee = 1,

    [Display(Name = "در انتظار امضای مدیران")]
    WaitingForManagerSignature = 1,

    //[Display(Name = "امضای مدیر پرسنل انجام شده")]
    //SignedByDirectManager = 3,

    [Display(Name = "در انتظار امضای معاون منابع انسانی")]
    WaitingForHRManagerSignature = 2,

    [Display(Name = "در انتظار امضای مدیرعامل")]
    WaitingForCEOSignature = 3
}