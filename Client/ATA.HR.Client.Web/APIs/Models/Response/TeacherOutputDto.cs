using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using ExcelWizard.Models;
using System.Drawing;

namespace ATA.HR.Client.Web.APIs.Models.Response;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست مدرسان", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class TeacherOutputDto
{
    [ExcelSheetColumn(Ignore = true)] 
    public long Id { get; set; }

    [ExcelSheetColumn(HeaderName = "نام", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string FullName { get; set; }

    [ExcelSheetColumn(HeaderName = "کد ملی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string NationalCode { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره تلفن", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string PhoneNumber { get; set; }
}