using System.Drawing;
using ATA.HR.Client.Web.APIs.Enums;
using ATABit.Helper.Extensions;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using EnumExtensions = Bit.Utils.Extensions.EnumExtensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست کودکان", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class ChildrenOutputDto
{
    [ExcelSheetColumn(Ignore = true)]
    public long Id { get; set; }

    [ExcelSheetColumn(HeaderName = "نام", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string FullName { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره ملی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string IdNo { get; set; }

    [ExcelSheetColumn(HeaderName = "نام پدر", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string FatherName { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime BirthDate { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ تولد", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? BirthDateJalali => BirthDate.ToJalaliString();

    [ExcelSheetColumn(HeaderName = "نام مادر", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string MotherName { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public ActivityEnum? Activity { get; set; }

    [ExcelSheetColumn(HeaderName = "سطح فعالیت", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string ActivityCaption => Activity?.ToDisplayName();

}