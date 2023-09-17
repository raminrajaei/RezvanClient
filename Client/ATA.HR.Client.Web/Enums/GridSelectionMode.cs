using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.Enums;

public enum GridSelectionMode
{
    [Display(Name = "همه‌ی آیتم‌ها‌‌")]
    SelectAll = 0,

    [Display(Name = "انتخابی")]
    CustomSelect = 1
}