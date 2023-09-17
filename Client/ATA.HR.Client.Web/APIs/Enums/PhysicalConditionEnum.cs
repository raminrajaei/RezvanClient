using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum PhysicalConditionEnum
{
    [Display(Name = "فعال")]
    Active,

    [Display(Name = "غیرفعال")]
    DeActive,

    [Display(Name = "مانند دیگران")]
    LikeOther
}