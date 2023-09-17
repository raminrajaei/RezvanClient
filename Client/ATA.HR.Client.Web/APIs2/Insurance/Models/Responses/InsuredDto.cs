using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using System.Drawing;

namespace ATA.HR.Client.Web.APIs.Insurance.Models;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست بیمه", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class InsuredDto
{
    [ExcelSheetColumn(Ignore = true)]
    public int Id { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CreatedAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ عضویت", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? CreatedAtJalali { get; set; }

    [ExcelSheetColumn(HeaderName = "وضعیت کلی بیمه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public bool IsActive { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int PolicyId { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? PolicyTitle { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int UserIdMainInsured { get; set; }

    [ExcelSheetColumn(HeaderName = "کد پرسنلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 12)]
    public int UserPersonnelCode { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "موبایل پرسنل", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? UserMobile { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "جنسیت", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public int? UserGender { get; set; } //Flattening 1=male | 2=female

    [ExcelSheetColumn(Ignore = true)]
    public string? UserSaderatAccountNo { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "حساب صادرات", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? UserSaderatAccountNoDisplay => IsMainInsurer ? UserSaderatAccountNo : null;

    [ExcelSheetColumn(Ignore = true)]
    public string? UserInsuranceCode { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "شماره بیمه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? UserInsuranceCodeDisplay => IsMainInsurer ? UserInsuranceCode : null;

    [ExcelSheetColumn(HeaderName = "خاتمه خدمت", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public bool UserDismissed { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "نسبت", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? RelationType { get; set; }
    //public string? RelationTypeDisplay => IsMainInsurer ? "اصلی" : RelationType;

    [ExcelSheetColumn(Ignore = true)]
    public bool IsMainInsurer { get; set; }

    [ExcelSheetColumn(HeaderName = "نام بیمه شده", ExcelDataContentType = CellContentType.General, ColumnWidth = 14)]
    public string? FirstName { get; set; }

    [ExcelSheetColumn(HeaderName = "خانوادگی بیمه شده", ExcelDataContentType = CellContentType.General, ColumnWidth = 14)]
    public string? LastName { get; set; }

    [ExcelSheetColumn(HeaderName = "نام کامل بیمه شده", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? FullName { get; set; }

    [ExcelSheetColumn(HeaderName = "کد ملی", ExcelDataContentType = CellContentType.General, ColumnWidth = 14)]
    public string? NationalCode { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? BirthDate { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ تولد", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? BirthDateJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? StartedAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ شروع", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? StartedAtJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? ExpireAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ انقضا بیمه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? ExpiredAtJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? EndedAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ قطع بیمه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? EndedAtJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int InsuranceStatus { get; set; }

    [ExcelSheetColumn(HeaderName = "وضعیت ب. تکمیلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 35)]
    public string? InsuranceStatusDisplay { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? Bank { get; set; }

    [ExcelSheetColumn(HeaderName = "بانک عامل", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? BankDisplay { get; set; }
}