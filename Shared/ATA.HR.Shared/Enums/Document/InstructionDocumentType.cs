using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Document;

public enum InstructionDocumentType
{
    [Display(Name = "آئین نامه‌ی استخدام پرسنل")]
    PersonnelEmploymentRegulations = 0,

    [Display(Name = "دستورالعمل جذب و جابجایی نیروی انسانی")]
    InstructionsForTheRecruitmentAndTransferOfHumanResources = 1,

    [Display(Name = "آئین نامه معاینات پزشکی کروی پروازی")]
    RegulationOfFlightCrewMedicalExaminations = 2,

    [Display(Name = "آئین نامه‌ی معاینات پزشکی کارکنان")]
    RegulationOfEmployeesMedicalExaminations = 3,

    [Display(Name = "آئین نامه حقوق و دستمزد کارکنان")]
    StaffSalaryRegulations = 4,

    [Display(Name = "دستورالعمل حقوق و دستمزد کروی پروازی")]
    FlightCrewSalaryGuidelines = 5,

    [Display(Name = "دستورالعمل صدور کارت پرسنلی")]
    InstructionsForIssuingPersonnelCard = 6,

    [Display(Name = "آئین نامه دوره‌های آموزشی")]
    RegulationsOfTrainingCourses = 7,

    [Display(Name = "دستورالعمل پرداخت حق التدریس")]
    TuitionPaymentInstructions = 8,

    [Display(Name = "آئین نامه انضباطی")]
    DisciplinaryRegulations = 9,

    [Display(Name = "دستورالعمل حضور و غیاب پرسنل")]
    PersonnelAttendanceGuidelines = 10,

    [Display(Name = "دستورالعمل ماموریت خارجی")]
    ForeignMissionInstructions = 11,

    [Display(Name = "دستورالعمل ماموریت داخلی")]
    DomesticMissionInstructions = 12,

    [Display(Name = "دستورالعمل مرخضی استحقاقی")]
    EntitledVacationInstructions = 13,

    [Display(Name = "دستورالعمل مرخصی استعلاجی")]
    InstructionsForSickLeave = 14,

    [Display(Name = "دستورالعمل مرخصی بدون حقوق")]
    GuidelinesForUnpaidLeave = 15,

    [Display(Name = "دستورالعمل مرخصی زایمان")]
    InstructionsForMaternityLeave = 16,

    [Display(Name = "دستورالعمل مرخصی زایمان مهمانداران")]
    MaternityLeaveInstructionsForFlightAttendants = 17,

    [Display(Name = "دستورالعمل مساعده حقوق")]
    SalaryAidGuidelines = 18,

    [Display(Name = "آئین نامه اعطای بلیت رایگان")]
    FreeTicketInstructions = 19,

    [Display(Name = "دستورالعمل کمک بلاعوض")]
    GrantGuidelines = 20,

    [Display(Name = "دستورالعمل هدایای مناسبتی")]
    InstructionsForSpecialGifts = 21,

    [Display(Name = "دستورالعمل صدور کارت ویزیت")]
    InstructionsForIssuingVisitCard = 22,

    [Display(Name = "آئین نامه اعطای یونیفرم")]
    RegulationsForGrantingUniforms = 23,

    [Display(Name = "دستورالعمل پرداخت هزینه تلفن همراه")]
    MobilePhonePaymentInstructions = 24,

    [Display(Name = "دستورالعمل بهره‌وری کارکنان")]
    EmployeeProductivityGuidelines = 25,

    [Display(Name = "آئین نامه تدوین قراردادنویسی")]
    CodeOfContractDrafting = 26,

    [Display(Name = "دستورالعمل تنظیم گزارش‌های آماری")]
    InstructionsForSettingUpStatisticalReports = 27,

    [Display(Name = "آئین نامه ارزیابی عملکرد پرسنل")]
    PersonnelPerformanceEvaluationRegulations = 28,

    [Display(Name = "دستورالعمل نگهداری خودرو")]
    CarMaintenanceInstructions = 29,

    [Display(Name = "دستورالعمل تاییده پزشکی مسافرین")]
    GuidelinesForMedicalApprovalOfPassengers = 30,

    [Display(Name = "آئین نامه گردش مکاتبات داخلی سازمان")]
    RegulationsForCirculationOfInternalCorrespondenceOfTheOrganization = 31,

    [Display(Name = "دستورالعمل نحوه تشکیل و ترکیب اعضا حفاظت فنی و بهداشت کار")]
    InstructionsOnHowToFormAndCombineMembersOfTechnicalProtectionAndOccupationalHealth = 32,

    [Display(Name = "آئین نامه کمیسیون امور اداری و منابع انسانی")]
    RegulationsOfTheAdministrativeAffairsAndHumanResourcesCommission = 33,

    [Display(Name = "دستورالعمل نحوه‌ی برخورد با مسافر متمرد و پرخاشگر")]
    InstructionsOnHowToDealWithUnrulyAndAggressivePassengers = 34,

    [Display(Name = "دستورالعمل مامورین حراست تحویل و تحول اشیا ممنوعه")]
    InstructionsForSecurityGuardsToDeliverAndTransformProhibitedItems = 35
}