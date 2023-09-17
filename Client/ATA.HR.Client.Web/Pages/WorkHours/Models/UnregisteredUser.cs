using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;

namespace ATA.HR.Client.Web.Pages.WorkHours.Models;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "پرسنل ثبت نشده")]
public class UnregisteredUser
{
    [ExcelSheetColumn(HeaderName = "شماره پرسنلی")]
    public int PersonnelCode { get; set; }

    [ExcelSheetColumn(HeaderName = "نام پرسنل")]
    public string? PersonnelFullName { get; set; }

    [ExcelSheetColumn(HeaderName = "سمت پرسنل")]
    public string? PostTitle { get; set; }
}