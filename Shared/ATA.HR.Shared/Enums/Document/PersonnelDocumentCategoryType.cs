using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Document;

public enum PersonnelDocumentCategoryType
{
    [Display(Name = "اطلاعات هویتی")]
    IdentityInformation = 0,

    [Display(Name = "مدارک استخدامی")]
    EmploymentDocuments = 1,

    [Display(Name = "احکام و قرارداد و ارزیابی عملکرد")]
    EmploymentContractAndHRRules = 2,

    [Display(Name = "مکاتبات اداری")]
    OfficialLettersAndCorrespondence = 4,

    [Display(Name = "تضمین و تعهد خدمتی")]
    WorkObligationAndGuarantee = 8,

    [Display(Name = "محاسبات مالی و تسویه حساب")]
    Financial = 16
}