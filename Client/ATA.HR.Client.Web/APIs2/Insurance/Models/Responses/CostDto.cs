using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using System.Drawing;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Responses;

[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "لیست هزینه", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class CostDto
{
    [ExcelSheetColumn(Ignore = true)]
    public int Id { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CreatedAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ ثبت", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? CreatedAtJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int UserIdRegistrar { get; set; }

    [ExcelSheetColumn(HeaderName = "نام اصلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? UserRegistrarFullName { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره پرسنلی اصلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? UserRegistrarPersonnelCode { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int InsuredId { get; set; }

    [ExcelSheetColumn(HeaderName = "نام بیمار", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? InsuredFullName { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int PolicyObligationId { get; set; }

    [ExcelSheetColumn(HeaderName = "کد عنوان", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public int PolicyObligationObligationCode { get; set; } 

    [ExcelSheetColumn(HeaderName = "نوع عنوان", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? PolicyObligationObligationTitle { get; set; }

    [ExcelSheetColumn(HeaderName = "عنوان هزینه", ExcelDataContentType = CellContentType.General, ColumnWidth = 35)]
    public string? Title { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CostDate { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ هزینه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? CostDateJalali { get; set; }

    [ExcelSheetColumn(HeaderName = "مبلغ هزینه", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public int PaidAmount { get; set; }

    [ExcelSheetColumn(HeaderName = "توضیحات", ExcelDataContentType = CellContentType.General, ColumnWidth = 25)]
    public string? Description { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? FranchisePercent { get; set; }

    [ExcelSheetColumn(HeaderName = "مبلغ تایید شده", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public int? ConfirmedAmount { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? FinalPayableAmount => CalculatePayableAmount(ConfirmedAmount, FranchisePercent);

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? ConfirmedAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ تایید", ExcelDataContentType = CellContentType.General, ColumnWidth = 15)]
    public string? ConfirmedAtJalali { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? BankPaymentId { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? FlowLastComment { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? FlowGeneralStatus { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? FlowGeneralStatusDisplay { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? FlowCurrentStateTag { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public List<CostAttachmentDto> CostAttachments { get; set; } = new();

    private int? CalculatePayableAmount(int? confirmedAmount, int? franchisePercent)
    {
        if (franchisePercent.HasValue && confirmedAmount.HasValue)
            return confirmedAmount - confirmedAmount * franchisePercent/100;

        return null;
    }

}