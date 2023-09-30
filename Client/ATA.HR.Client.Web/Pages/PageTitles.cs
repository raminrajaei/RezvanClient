using ATA.HR.Shared;

namespace ATA.HR.Client.Web.Pages;

public static class PageTitles
{
    // Rezvan
    #region Child
    public static class ChildStudentsPage
    {
        public static readonly string Title = "لیست کودکان";
    }

    public static class ChildPrintPage
    {
        public static readonly string Title = "جزییات کودک";
    }

    public static class AddChildStudentFormPage
    {
        public static readonly string Title = "ثبت کودک جدید";
    }

    public static class EditChildStudentFormPage
    {
        public static readonly string Title = "ویرایش کودک";
    }
    #endregion

    #region Adult
    public static class AdultStudentsPage
    {
        public static readonly string Title = "لیست بزرگسالان";
    }

    public static class AdultPrintPage
    {
        public static readonly string Title = "جزییات بزرگسال";
    }

    public static class AddAdultStudentFormPage
    {
        public static readonly string Title = "ثبت بزرگسال جدید";
    }

    public static class EditAdultStudentFormPage
    {
        public static readonly string Title = "ویرایش بزرگسال";
    }
    #endregion

    #region Teacher
    public static class TeacherPage
    {
        public static readonly string Title = "لیست مدرسان";
    }

    public static class TeacherPrintPage
    {
        public static readonly string Title = "جزییات مدرس";
    }

    public static class AddTeacherFormPage
    {
        public static readonly string Title = "ثبت مدرس جدید";
    }

    public static class EditTeacherFormPage
    {
        public static readonly string Title = "ویرایش مدرس";
    }
    #endregion

    #region Class
    public static class ClassPage
    {
        public static readonly string Title = "لیست کلاس‌ها";
    }
    #endregion

    #region Child Class
    public static class ChildrenPage
    {
        public static readonly string Title = "لیست کلاس های کودکان";
    }
    public static class AddChildClassFormPage
    {
        public static readonly string Title = "ثبت کلاس کودک";
    }
    public static class EditChildClassFormPage
    {
        public static readonly string Title = "ویرایش کلاس کودک";
    }
    #endregion

    #region Adult Class
    public static class AdultsPage
    {
        public static readonly string Title = "لیست کلاس های بزرگسالان";
    }
    public static class AddAdultClassFormPage
    {
        public static readonly string Title = "ثبت کلاس بزرگسال";
    }
    public static class EditAdultClassFormPage
    {
        public static readonly string Title = "ویرایش کلاس بزرگسال";
    }
    #endregion

    // ATA: To Remove Soon
    public static class HomePage
    {
        public static readonly string Title = AppMetadata.PersianFullName;
    }

    public static class DashboardPage
    {
        public static readonly string Title = "کارپوشه شما – سامانه جامع منابع انسانی";
    }

    public static class ContractFlowFormsPage
    {
        public static readonly string TitleMain = "قرارداد کار";
        public static readonly string TitlePreview = "مشاهده‌ی پیش نمایش قرارداد کار";
    }

    public static class WorkHoursFlowFormsPage
    {
        public static readonly string TitleMain = "کارکرد پرسنل";
    }
    
    public static class InsuranceFlowFormsPage
    {
        public static readonly string TitleMain = "هزینه بیمه تکمیلی";
    }

    public static class LogsPage
    {
        public static readonly string Title = "Audit Logs";
    }

    public static class ConfirmManagerIdentityPage
    {
        public static readonly string Title = "تایید دو مرحله‌ای مدیران آتا";
    }

    public static class SendContractPage
    {
        public static readonly string Title = "ارسال قرارداد";
    }

    public static class UsersPage
    {
        public static readonly string Title = "کاربران";
    }

    public static class RolesPage
    {
        public static readonly string Title = "نقش‌ها";
    }

    public static class SettingsPage
    {
        public static readonly string Title = "تنظیمات";
    }

    public static class MyContractsPage
    {
        public static readonly string Title = "قراردادهای من";
    }

    public static class AllContractsPage
    {
        public static readonly string Title = "همه‌ی قراردادها";
    }

    public static class ContractsDirectManagersPage
    {
        public static readonly string Title = "مدیران مستقیم قراردادها";
    }

    public static class WorkHoursDirectManagersPage
    {
        public static readonly string Title = "مدیران مستقیم کارکردها";
    }

    public static class SignaturesPage
    {
        public static readonly string Title = "امضای کاربران";
    }

    public static class DocUsersPage
    {
        public static readonly string Title = "بایگانی الکترونیکی پرونده‌ی سامانه‌ی استخدامی پرسنل";
    }

    public static class PersonnelDocumentsPage
    {
        public static readonly string Title = "بایگانی الکترونیکی";
    }

    public static class BuildingPage
    {
        public static readonly string Title = "ساختمان";
    }

    public static class UnitPage
    {
        public static readonly string Title = "واحد";
    }

    public static class RoomPage
    {
        public static readonly string Title = "اتاق";
    }

    public static class BookingPage
    {
        public static readonly string Title = "مشاهده و رزرو اتاق ها";
    }

    public static class BookingFormPage
    {
        public static readonly string Title = "رزرو";
    }

    public static class InstructionDocumentsPage
    {
        public static readonly string Title = "بایگانی بخش‌نامه‌ها و دستورالعمل‌ها";
    }

    public static class CommitmentLettersPage
    {
        public static readonly string Title = "تعهدنامه‌های محضری پرسنل";
    }

    public static class AllWorkHoursPage
    {
        public static readonly string Title = "کارکرد پرسنل";
    }

    public static class WorkHoursForCeoConfirmReportPage
    {
        public static readonly string Title = "گزارش کارکردهای ماهانه";
    }
    
    public static class CostsPrintPage
    {
        public static readonly string Title = "پرینت هزینه‌ها";
    }
    
    public static class FinancialReportPage
    {
        public static readonly string Title = "لیست پرداخت بیمه تکمیلی";
    }
    
    public static class MyCostsPrintPage
    {
        public static readonly string Title = "پرینت هزینه‌ها";
    }

    public static class DismissSmsesPage
    {
        public static readonly string Title = "مدیریت پیام‌های خاتمه خدمت";
    }

    public static class AllAppSMSesPage
    {
        public static readonly string Title = "همه‌ی پیامک‌های ارسالی از سیستم";
    }

    public static class NotifyManagersWorkHourJobSettingsPage
    {
        public static readonly string Title = "تنظیمات پیامک هوشمند اطلاع رسانی به مدیران برای تایید کارکردها";
    }

    public static class PaySlipPage
    {
        public static readonly string Title = "فیش حقوقی";
    }

    public static class InsuranceCoveragePoliciesPage
    {
        public static readonly string Title = "تعرفه‌های بیمه تکمیلی";
    }

    public static class MyInsuredPeoplePage
    {
        public static readonly string Title = "مشخصات بیمه شده‌ی اصلی و تحت تکفل";
    }

    public static class MyCostsPage
    {
        public static readonly string Title = "هزینه‌های من";
    }

    public static class AddNewCostPage
    {
        public static readonly string Title = "افزودن هزینه‌ی جدید";
    }

    public static class EditCostPage
    {
        public static readonly string Title = "ویرایش هزینه‌";
    }

    public static class AllCostsPage
    {
        public static readonly string Title = "لیست اسناد هزینه‌";
    }

    public static class AllInsuredsPage
    {
        public static readonly string Title = "لیست همه بیمه شدگان";
    }
    
   
}