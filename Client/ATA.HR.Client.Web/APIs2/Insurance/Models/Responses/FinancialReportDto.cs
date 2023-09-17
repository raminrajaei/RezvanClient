using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Responses;

[ComplexType]
[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "کارکرد پرسنل", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class FinancialReportDto
{
    [ExcelSheetColumn(HeaderName = "ردیف", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public int RowNo { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره پرسنلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public int PersonnelCode { get; set; }

    [ExcelSheetColumn(HeaderName = "نام اصلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? MainInsuredName { get; set; }

    [ExcelSheetColumn(HeaderName = "مبلغ (ریال)", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public int Amount { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره حساب", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? BankAccountNo { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره شبا", ExcelDataContentType = CellContentType.General, ColumnWidth = 33)]
    public string? BankShebaNo { get; set; }
}