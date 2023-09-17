using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.AppGeneric;

public enum AppModule
{
    [Display(Name = "ارسال هوشمند پیامک به مدیران مستقیم برای تایید کارکردها")]
    SendJobSmsToDirectManagersToConfirmWorkHoursInTheirToDoDashboard = 1
}