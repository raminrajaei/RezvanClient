using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using ExcelWizard.Models;
using System.Drawing;
using ATABit.Helper.Extensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست کلاس های کودکان", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class ChildClassOutputDto
{
    [ExcelSheetColumn(Ignore = true)]
    public long Id { get; set; }

    [ExcelSheetColumn(HeaderName = "نام", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string ChildFullName { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره شناسنامه", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string ChildIdNo { get; set; }

    [ExcelSheetColumn(HeaderName = "نام پدر", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string ChildFatherName { get; set; }

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

public static class ContractExtensions
{
    public static string ToRialDisplay(this int digit)
    {
        if (digit == 0)
            return "0";

        return Convert.ToInt32(digit).ToCurrencyStringFormat().En2FaDigits();
    }
}