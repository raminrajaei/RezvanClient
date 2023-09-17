// ReSharper disable InconsistentNaming

using Bit.Core.Exceptions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ATA.HR.Shared;

public class DefaultRole
{
    public string? Name { get; set; }

    public IEnumerable<string> Claims { get; set; } = Array.Empty<string>();
}

public class DefaultRoles
{
    public static DefaultRole Administrator => new()
    {
        Name = nameof(Administrator),
        Claims = Claims.GetAllAppClaims()
    };
}

public static class Claims //* Do not change Claim values *//
{
    public const string Settings_Permissions_View = nameof(Settings_Permissions_View);
    public const string Settings_Permissions_Manage = nameof(Settings_Permissions_Manage);
    // Contracts
    public const string Pages_Contracts_StartNewContractFlow = nameof(Pages_Contracts_StartNewContractFlow);
    public const string AccessToAllPersonnelContracts = nameof(AccessToAllPersonnelContracts);
    public const string GatherSignaturesPermission = nameof(GatherSignaturesPermission);
    public const string DirectManagersListView = nameof(DirectManagersListView);
    // Docs
    public const string Docs_IdentityInformation = nameof(Docs_IdentityInformation);
    public const string Docs_EmploymentDocuments = nameof(Docs_EmploymentDocuments);
    public const string Docs_EmploymentContractAndHRRules = nameof(Docs_EmploymentContractAndHRRules);
    public const string Docs_OfficialLettersAndCorrespondence = nameof(Docs_OfficialLettersAndCorrespondence);
    public const string Docs_WorkObligationAndGuarantee = nameof(Docs_WorkObligationAndGuarantee);
    public const string Docs_DeletePermission = nameof(Docs_DeletePermission);
    public const string Docs_Instructions = nameof(Docs_Instructions);
    // Commitment Letter
    public const string CommitmentLetters_AddAndView = nameof(CommitmentLetters_AddAndView);
    public const string CommitmentLetters_OnlyView = nameof(CommitmentLetters_OnlyView);
    public const string CommitmentLetters_DeletePermission = nameof(CommitmentLetters_DeletePermission);
    // WorkHours
    public const string WorkHours_RegisterOrEdit = nameof(WorkHours_RegisterOrEdit);
    public const string WorkHours_Edit = nameof(WorkHours_Edit);
    public const string WorkHours_AllowExtraBT120 = nameof(WorkHours_AllowExtraBT120);
    public const string WorkHours_ViewAll = nameof(WorkHours_ViewAll);
    public const string WorkHours_Delete = nameof(WorkHours_Delete);
    public const string WorkHours_StartFlow = nameof(WorkHours_StartFlow);
    public const string WorkHours_Reports = nameof(WorkHours_Reports);
    public const string WorkHours_SendSmsToAllManagers = nameof(WorkHours_SendSmsToAllManagers);

    // SMS
    public const string AppSMSes_ViewAll = nameof(AppSMSes_ViewAll);

    // Insurance
    public const string Insurance_Calculations = nameof(Insurance_Calculations);
    public const string Insurance_ViewAllMembers = nameof(Insurance_ViewAllMembers);
    public const string Insurance_TerminateInsured = nameof(Insurance_TerminateInsured);

    // Pages_Guest_House
    public const string Pages_Guest_House_Management_Tehran = nameof(Pages_Guest_House_Management_Tehran);
    public const string Pages_Guest_House_Management_Tabriz = nameof(Pages_Guest_House_Management_Tabriz);

    private static string[]? _claimNames;

    public static string GetClaimDisplayName(string claimType)
    {
        return claimType switch
        {
            Settings_Permissions_View => "مشاهده‌ی کاربران و نقش‌های سامانه",
            Settings_Permissions_Manage => "مدیریت دسترسی‌ نقش‌ها",
            Pages_Contracts_StartNewContractFlow => "ارسال قرارداد جدید پرسنل",
            AccessToAllPersonnelContracts => "دسترسی به قراردادهای کل پرسنل",
            GatherSignaturesPermission => "جمع‌آوری امضا سایر پرسنل",
            DirectManagersListView => "مشاهده‌ی لیست مدیران مستقیم تایید کننده",
            Docs_IdentityInformation => "مشاهده/آپلود مستندات اطلاعات هویتی",
            Docs_EmploymentDocuments => "مشاهده/آپلود مستندات مدارک استخدامی",
            Docs_EmploymentContractAndHRRules => "مشاهده/آپلود مستندات احکام و قراردادها",
            Docs_OfficialLettersAndCorrespondence => "مشاهده/آپلود مستندات مکاتبات اداری",
            Docs_WorkObligationAndGuarantee => "مشاهده/آپلود مستندات تضمین و تعهدات خدمتی",
            Docs_DeletePermission => "دسترسی حذف مستندات",
            Docs_Instructions => "مشاهده/آپلود مستندات بخش‌نامه‌ها و دستورالعمل‌ها",
            CommitmentLetters_AddAndView => "مشاهده/افزودن تعهدات محضری",
            CommitmentLetters_OnlyView => "فقط مشاهده تعهدات محضری",
            CommitmentLetters_DeletePermission => "حذف تعهدات محضری",
            Pages_Guest_House_Management_Tehran => "مدیریت مهمانسرای تهران",
            Pages_Guest_House_Management_Tabriz => "مدیریت مهمانسرای تبریز",
            WorkHours_StartFlow => "شروع گردش کار کارکردها",
            WorkHours_RegisterOrEdit => "ثبت کارکرد",
            WorkHours_Edit => "ویرایش کارکرد",
            WorkHours_ViewAll => "مشاهده همه‌ی کارکردها",
            WorkHours_Delete => "حذف کارکرد",
            WorkHours_Reports => "مشاهده‌ی گزارشات کارکردها",
            WorkHours_SendSmsToAllManagers => "ارسال پیامک اطلاع رسانی تایید کارکردها به مدیران",
            WorkHours_AllowExtraBT120 => "ثبت اضافه کاری بالای 120 ساعت",
            AppSMSes_ViewAll => "مشاهده‌ی بایگانی پیامک‌های ارسالی",
            Insurance_Calculations => "محاسبات بیمه تکمیلی",
            Insurance_ViewAllMembers => "مشاهده همه بیمه شدگان",
            Insurance_TerminateInsured => "لغو عضویت بیمه تکمیلی پرسنل",
            _ => throw new DomainLogicException("همه Claimها باید مقدار نمایشی داشته باشند.")
        };
    }

    public static IEnumerable<string> GetAllAppClaims()
    {
        return _claimNames ??= typeof(Claims)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Select(field => (string?)field.GetValue(null))
            .ToArray();
    }

    // Automatic Check for all Claims have DisplayName With Module Initializer
    [ModuleInitializer]
    public static void AutomaticCheckAllClaimsHaveDisplayName()
    {
        var allClaims = GetAllAppClaims();

        foreach (var claim in allClaims)
        {
            GetClaimDisplayName(claim); //Throw error if no display is assigned for the Claim.
        }
    }
}