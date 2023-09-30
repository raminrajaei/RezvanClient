using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using ExcelWizard.Models;
using System.Drawing;
using ATABit.Helper.Extensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست کلاس های بزرگسالان", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class AdultClassOutputDto
{
    [ExcelSheetColumn(Ignore = true)]
    public long Id { get; set; }

    [ExcelSheetColumn(HeaderName = "نام", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string AdultFullName { get; set; }

    [ExcelSheetColumn(HeaderName = "کد ملی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string AdultNationalCode { get; set; }

    [ExcelSheetColumn(HeaderName = "نام کلاس", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string ClassRoomTitle { get; set; }

    [ExcelSheetColumn(HeaderName = "نام مربی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string TeacherFullName { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int Tuition { get; set; }

    [ExcelSheetColumn(HeaderName = "شهریه", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string RialTuition => Tuition.ToRialDisplay();

    [ExcelSheetColumn(Ignore = true)]
    public DateTime From { get; set; }

    [ExcelSheetColumn(HeaderName = "از تاریخ", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? FromDateJalali => From.ToJalaliString();

    [ExcelSheetColumn(Ignore = true)]
    public DateTime To { get; set; }

    [ExcelSheetColumn(HeaderName = "تا تاریخ", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? ToDateJalali => To.ToJalaliString();
}