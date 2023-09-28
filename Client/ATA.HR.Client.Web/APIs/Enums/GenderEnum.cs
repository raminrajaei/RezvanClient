using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum GenderEnum
{
    [Display(Name = "مرد")]
    Man = 1,

    [Display(Name = "زن")]
    Woman = 2
}