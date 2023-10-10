using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum ActivityEnum
{
    [Display(Name = "معمولی")]
    Normal,

    [Display(Name = "نقره ای")]
    Silver,

    [Display(Name = "طلایی")]
    Golden
}