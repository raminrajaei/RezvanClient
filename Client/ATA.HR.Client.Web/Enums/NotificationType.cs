using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.Enums;

public enum NotificationType
{
    [Display(Name = "Success")]
    Success,

    [Display(Name = "Error")]
    Error,

    [Display(Name = "Message")]
    Message,

    [Display(Name = "Notify")]
    Notify,

    [Display(Name = "Warning")]
    Warning
}