using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.AppGeneric.DbAppSettings;

[ComplexType]
public class NotifyDirectManagersToConfirmWorkHoursDbSettings
{
    public bool IsEnabled { get; set; }

    [Required(ErrorMessage = "متن پیامک را وارد نمایید")]
    public string? SmsContent { get; set; }

    public List<int> ExcludedManagers { get; set; } = new(); //UserIds Like CEO

    public List<string> MobilesToNotifyWhenAManagerIgnores { get; set; } = new();
}