using System.Drawing;
using ATABit.Helper.Extensions;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;

namespace ATA.HR.Client.Web.APIs.Models.Response;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست بزرگسالان", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20,
    HeaderHeight = 20)]
public class AdultOutputDto
{
    [ExcelSheetColumn(Ignore = true)] 
    public long Id { get; set; }

    [ExcelSheetColumn(HeaderName = "نام و نام خانوادگی", ExcelDataContentType = CellContentType.General,
        ColumnWidth = 20)]
    public string FullName { get; set; }

    [ExcelSheetColumn(HeaderName = "کد ملی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string NationalCode { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime BirthDate { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ تولد", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? BirthDateJalali => BirthDate.ToJalaliString();
}