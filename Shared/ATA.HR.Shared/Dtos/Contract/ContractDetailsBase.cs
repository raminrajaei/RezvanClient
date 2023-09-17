using ATA.HR.Shared.Enums.Contract;
using ATABit.Helper.Extensions;

namespace ATA.HR.Shared.Dtos.Contract;

public class ContractDetailsBase
{
    public string? PersonnelCode { get; set; }

    public string? NationalID { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? FatherName { get; set; }

    public string? Shenasname { get; set; }

    // [محل صدور]
    public string? BirthCertificateIssuingPlace { get; set; }

    public string? BirthCity { get; set; }

    public string? BirthDateJalali { get; set; }

    public string? MilitaryServiceStatus { get; set; }

    public string? EducationDegree { get; set; }

    public string? EducationField { get; set; }

    public string? WorkingPlace { get; set; }

    public string? EmploymentDate { get; set; }

    public string? MaritualStatus { get; set; }

    public int ChildrenNo { get; set; }

    public string? Unit { get; set; }

    // [عنوان شغل]
    public string? JobTitle { get; set; }

    public string? JobCode { get; set; }

    // [عنوان پست]
    public string? PostTitle { get; set; }

    public string? PostCode { get; set; }

    // [گروه]
    public int? Group { get; set; }

    // [رتبه]
    public int? Rating { get; set; }

    // [طبقه]
    public int? Level { get; set; }

    // [نوع حكم]
    public string? HokmType { get; set; }

    // [نوع استخدام]
    public long? EmploymentTypeCode { get; set; }
    public string? EmploymentTypeCodeDisplay => EmploymentTypeCode.HasValue ? ((EmploymentType)(int)EmploymentTypeCode).ToDisplayName() : "نا مشخص";

    // [حق مسکن]
    public decimal? HousingAllowance { get; set; }

    // [حق اولاد]
    public decimal? ChildrenAllowance { get; set; }

    // [مزد شغل]
    public decimal? JobWage { get; set; }

    // [پایه سنوات]
    public decimal? YearsBasePay { get; set; }

    // [حق مسئولیت]
    public decimal? ResponsibilityRight { get; set; }

    /// [فوق العاده گواهینامه]
    public decimal? LicenceRight { get; set; }

    // [پاداش اعطایی مدیریت]
    public decimal? CEOReward { get; set; }

    // [جق جذب]
    public decimal? EmploymentRight { get; set; }

    // [راندمان و کارایی]
    public decimal? EfficiencyRight { get; set; }

    // [حق خواروبار]
    public decimal? GroceryAllowance { get; set; }

    // [حق ایاب و ذهاب]
    public decimal? TransportationRight { get; set; }

    // [فوق العاده ویژه]
    public decimal? SpecialRight { get; set; }

    // [مزد مبنا]
    public decimal? BaseWage { get; set; }

    // [افزایش ثابت سال]
    public decimal? YearConstantIncrease { get; set; }

    // [فوق العاده شرایط احراز]
    public decimal? ExcellentQualification { get; set; }

    // [سایر1]
    public decimal? Other1 { get; set; }

    // [سایر2]
    public decimal? Other2 { get; set; }

    // [سایر3]
    public decimal? Other3 { get; set; }

    // [جمع حکم]
    public decimal? HokmSum { get; set; }

    // [نرخ سا عتی]
    public decimal? HourlyRate { get; set; }

    // [شرح حکم]
    public string? HokmDescription { get; set; }

    // [تاریخ صدور]
    public string? IssuanceDateJalali { get; set; }

    // [تاریخ اجرا]
    public DateTime? ExecutionDate { get; set; }

    // [تاریخ اجرا]
    //public string? ApplyDateJalali { get; set; }

    // [تاريخ اعتبار]
    public string? ValidityDateJalali { get; set; }
}