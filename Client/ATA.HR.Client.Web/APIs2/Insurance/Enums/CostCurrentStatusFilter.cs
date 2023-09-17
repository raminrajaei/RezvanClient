using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum CostCurrentStatusFilter
{
    [Display(Name = "در مرحله بررسی منابع انسانی")]
    WaitingHRCheck = 1,
    
    [Display(Name = "در مرحله رفع نقص مدارک")]
    EditCost = 0,

    [Display(Name = "تایید شده‌ی منابع انسانی")]
    HRConfirmed = 2,

    [Display(Name = "کارشناسی صندوق بیمه کوثر")]
    WaitingClaimsAdjusterAppraisal = 3,

    [Display(Name = "تایید شده صندوق بیمه")]
    CostAmountConfirmed = 4,

    [Display(Name = "در مرحله‌ی پرداخت")]
    WaitingForPayment = 5,

    [Display(Name = "پایان – پرداخت شده")]
    PaidAndFinish = 6
}