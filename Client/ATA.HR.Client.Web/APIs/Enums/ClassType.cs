using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum ClassType
{
    [Display(Name = "کودکان")]
    Child = 1,

    [Display(Name = "عمومی")]
    Public = 2,

    [Display(Name = "تخصصی")]
    Professional = 3,

    [Display(Name = "کادر متخصص")]
    Expert = 4
}