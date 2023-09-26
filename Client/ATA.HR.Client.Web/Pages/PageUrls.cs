namespace ATA.HR.Client.Web.Pages;

public static class PageUrls
{
    // Student
    public const string ChildRootPath = "/child";
    public const string ChildrenPage = $"{ChildRootPath}/all";
    public static string ChildPrintPage(int childId) => $"{ChildRootPath}/{childId}";
    public static string AddChildFormPage() => $"{ChildRootPath}/add";
    public static string EditChildFormPage(int childId) => $"{ChildRootPath}/edit/{childId}";

    public const string AdultRootPath = "/adult";
    public const string AdultsPage = $"{AdultRootPath}/all";
    public static string AdultPrintPage(int adultId) => $"{AdultRootPath}/{adultId}";
    public static string AddAdultFormPage() => $"{AdultRootPath}/add";
    public static string EditAdultFormPage(int adultId) => $"{AdultRootPath}/edit/{adultId}";

    // Teacher
    public const string TeacherRootPath = "/teacher";
    public const string TeachersPage = "/teachers";
    public static string AddTeacherFormPage() => $"{TeacherRootPath}/teacher";
    public static string EditTeacherFormPage(int teacherId) => $"{TeacherRootPath}/teacher/{teacherId}";

    // HR
    public const string Dashboard = "/";

    public static string ContractFlowFormsPage(int paramContractId) => $"/flow-forms/contract/{paramContractId}";
    public static string ContractFlowFormsPreviewPage(int paramContractId) => $"/flow-forms/contract/{paramContractId}/preview";

    public static string WorkHoursFlowFormsPage(int paramWorkHoursId) => $"/flow-forms/work-hours/{paramWorkHoursId}";
    public static string InsuranceCostFlowFormsPage(int paramCostId, bool fromAllCostsPage = false) => fromAllCostsPage is false
    ? $"/flow-forms/insurance/{paramCostId}"
    : $"/flow-forms/insurance/{paramCostId}/1";

    public const string LogsPage = "/logs";

    public const string ConfirmManagerIdentityPage = "/confirm-manager-identity";

    public const string ContractsRootPath = "/contracts";
    public const string MyContractsPage = $"{ContractsRootPath}/my-contracts";
    public const string SendContractPage = $"{ContractsRootPath}/send-contract";
    public const string AllContractsPage = $"{ContractsRootPath}/all-contracts";
    public const string DismissSmsesPage = $"{ContractsRootPath}/dismiss-smses";

    public const string GuestHousesRootPath = "/guest-houses";
    public static string BuildingPage(string city) => $"{GuestHousesRootPath}/building/{city}";
    public static string UnitPage(int buildingId) => $"{GuestHousesRootPath}/unit/{buildingId}";
    public static string RoomPage(int buildingId, int unitId) => $"{GuestHousesRootPath}/room/{buildingId}/{unitId}";
    public static string BookingPage(string city) => $"{GuestHousesRootPath}/booking/{city}";
    public static string BookingFormPage(string city, int roomId) => $"{GuestHousesRootPath}/booking-form/{city}/{roomId}";
    public static string BookingFormPage() => $"{GuestHousesRootPath}/booking-form";

    public const string SettingsRootPath = "/settings";
    public const string UsersPage = $"{SettingsRootPath}/users";
    public const string RolesPage = $"{SettingsRootPath}/roles";
    public const string SignaturesPage = $"{SettingsRootPath}/signatures";
    public const string WorkflowPage = $"{SettingsRootPath}/workflow";
    public const string ContractsDirectManagersPage = $"{SettingsRootPath}/contracts-direct-managers";
    public const string WorkHoursDirectManagersPage = $"{SettingsRootPath}/workhours-direct-managers";
    public const string AllAppSMSesPage = $"{SettingsRootPath}/all-app-smses";
    public const string NotifyManagersWorkHourJobSettingsPage = $"{SettingsRootPath}/job-notify-managers-confirm-workhours";

    public const string DocumentsRootPath = "/docs";
    public const string DocUsersPage = $"{DocumentsRootPath}/users";
    public static string PersonnelDocumentsPage(int paramUserId) => $"{DocumentsRootPath}/user-docs/{paramUserId}";
    public static string PersonnelDocumentsPageWithParams(int paramUserId, int paramDocCategory, int paramSubCategoryId) => $"{DocumentsRootPath}/user-docs/{paramUserId}/{paramDocCategory}/{paramSubCategoryId}";
    public static string PersonnelDocumentsPageWithParams(int paramUserId, int paramDocCategory, int paramSubCategoryId, bool isNavigatedFromCommitmentLettersPage)
    {
        var isNavigatedFromCommitmentLetters = isNavigatedFromCommitmentLettersPage ? "1" : "0";
        return $"{DocumentsRootPath}/user-docs/{paramUserId}/{paramDocCategory}/{paramSubCategoryId}/{isNavigatedFromCommitmentLetters}";
    }
    public static string PersonnelDocumentsPageWithParams(int paramUserId, int paramDocCategory) => $"{DocumentsRootPath}/user-docs/{paramUserId}/{paramDocCategory}";

    public const string InstructionDocumentsPage = $"{DocumentsRootPath}/instructions";

    public const string CommitmentLettersRootPath = "/commitment-letters";
    public const string CommitmentLettersPage = $"{CommitmentLettersRootPath}/all-cl";

    public const string WorkHoursRootPath = "/workhours";
    public const string WorkHoursPage = $"{WorkHoursRootPath}/all";
    public const string WorkHoursReportToGetCEOConfirmationPage = $"{WorkHoursRootPath}/ceo-report";

    public const string InsuranceRootPath = "/insurance";
    public const string InsuranceCoverageViewPage = $"{InsuranceRootPath}/coverage-policies";
    public const string MyInsuredPeoplePage = $"{InsuranceRootPath}/my-insured-people";
    public const string AddNewCostPage = $"{InsuranceRootPath}/my-costs/1";
    public const string MyCostsPage = $"{InsuranceRootPath}/my-costs";
    public const string AllInsuredsPage = $"{InsuranceRootPath}/all-insureds";
    public const string AllCostsPage = $"{InsuranceRootPath}/all-costs";
    public const string MyCostsPrintPage = $"{InsuranceRootPath}/my-costs-print";
    public const string AllCostsPrintPage = $"{InsuranceRootPath}/my-costs-print/allcosts";
    public const string FinancialReportPage = $"{InsuranceRootPath}/financial-report";

    public const string PaySlipPage = $"payslip/view";

}