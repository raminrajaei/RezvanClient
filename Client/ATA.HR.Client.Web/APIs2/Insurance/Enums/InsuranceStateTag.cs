using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum InsuranceStateTag //Sync it with same Enum in Insurance micro-service
{
    [Display(Name = "ثبت هزینه")]
    CostRegister,

    [Display(Name = "منابع انسانی – تهران")]
    HRCheckTehran,

    [Display(Name = "منابع انسانی – تبریز")]
    HRCheckTabriz,

    [Display(Name = "منابع انسانی – مشهد")]
    HRCheckMashhad,

    [Display(Name = "رفع نقص مدارک – متقاضی")]
    CostEdit,

    [Display(Name = "کارشناسی هزینه")]
    ClaimsAdjusterAppraisal,

    [Display(Name = "پرداخت هزینه  – صندوق بیمه کوثر")]
    KosarInsurancePayment,

    [Display(Name = "پایان")]
    Finish
}