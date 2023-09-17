using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.AppGeneric;

public enum AppSMSType
{
    [Display(Name = "ارسال کد تایید برای امضای قرارداد")]
    SendCodeToSignContract = 1,

    [Display(Name = "ارسال قرارداد جدید")]
    NewContractNotification = 2,

    [Display(Name = "درخواست مجدد امضای قرارداد امضا نشده")]
    AskToSignOverdueContract = 3,

    [Display(Name = "پیام خاتمه‌ی خدمت")]
    NotifyYouAreDismissed = 4,

    [Display(Name = "ارسال کد تایید دو مرحله‌ای برای لاگین مدیران")]
    SendCodeToConfirmManagerIdentity = 5,

    [Display(Name = "پیامک هوشمند درخواست تایید کارکردها برای مدیران مستقیم")]
    JobNotifyDirectManagersToConfirmWorkHours = 6,

    [Display(Name = "ذخیره سازی روزانه‌ی جایگاه‌های کاربران در راهکاران")]
    JobSyncUserBoxesHistoryTable = 7,

    [Display(Name = "بروزرسانی کارکردهای مدیر مستقیم به منظور نمایش در گزارش")]
    JobFixDirectManagersWorkHoursForReport = 8,

    [Display(Name = "ارسال خودکار پیامک خاتمه خدمت")]
    JobSendDismissSms = 9
}

public static class AppSMSTypeExtensions
{
    public static bool IsBackgroundJob(this AppSMSType appSMSType)
    {
        return appSMSType switch
        {
            AppSMSType.JobNotifyDirectManagersToConfirmWorkHours => true,
            AppSMSType.JobSyncUserBoxesHistoryTable => true,
            AppSMSType.JobFixDirectManagersWorkHoursForReport => true,
            AppSMSType.JobSendDismissSms => true,
            _ => false
        };
    }
}