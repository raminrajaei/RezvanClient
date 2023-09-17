using ATABit.Helper.Extensions;
using ATABit.Shared;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "قرارداد پرسنل", BorderType = LineStyle.Thin,
    BorderColor = KnownColor.Black, IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class ContractReadDto
{
    [ExcelSheetColumn(HeaderName = "شناسه قرارداد", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public int Id { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int UserId { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public bool IsPreview { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? ContractVersion { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime RequestedAt { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int? FlowStatus { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? FlowStatusDisplay => FlowStatus.HasValue ? ((Enums.Workflow.FlowStatus)FlowStatus).ToDisplayName() : "";

    [ExcelSheetColumn(Ignore = true)]
    public string? EmployeeSignatureImageDataURL { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? DirectManagerSignatureImageDataURL { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? DirectManagerSignedBy { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? HRSignatureImageDataURL { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? HRSignedBy { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? CEOSignatureImageDataURL { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? CEOSignedBy { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? WorkflowCurrentStateTag { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public ContractDetailsReadDto? ContractDetails { get; set; }

    // Flattening to show in grid

    [ExcelSheetColumn(Ignore = true)]
    public string? ContractDetailsPostTitle { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public string? ContractDetailsValidityDateJalali { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "تاریخ پایان", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? ContractDetailsValidityDateJalaliDisplay => ContractDetailsValidityDateJalali?.En2FaDigits();

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? ContractDetailsExecutionDate { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "تاریخ شروع", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? ContractDetailsExecutionDateJalaliDisplay => ContractDetailsExecutionDate?.ToJalaliString().En2FaDigits();

    [ExcelSheetColumn(HeaderName = "نام پرسنل", ExcelDataContentType = CellContentType.General, ColumnWidth = 25)]
    public string? ContractDetailsFullName { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "واحد", ExcelDataContentType = CellContentType.General, ColumnWidth = 25)]
    public string? ContractDetailsUnit { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public long? ContractDetailsEmploymentTypeCode { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public string? ContractDetailsPersonnelCode { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "کد پرسنلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 20)]
    public string? ContractDetailsPersonnelCodeDisplay => ContractDetailsPersonnelCode?.En2FaDigits();

    [ExcelSheetColumn(Ignore = true)]
    public string? ContractDetailsEmploymentDate { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public string? ContractDetailsEmploymentDateDisplay => ContractDetailsEmploymentDate?.En2FaDigits();

    [ExcelSheetColumn(Ignore = true)]
    public UserDto? User { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public bool IsUserDismissed { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? UserMobile { get; set; } //Flattening to show in grid

    [ExcelSheetColumn(Ignore = true)]
    public string? UserBoxId { get; set; } //Flattening to show in grid

    [ExcelSheetColumn(Ignore = true)]
    public string? UserWorkLocation { get; set; } //Flattening to show in grid

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? UserBirthDate { get; set; } //Flattening to show in grid

    [ExcelSheetColumn(Ignore = true)]
    public string UserBirthDateJalali => UserBirthDate.ToJalaliString();
}

public static class ContractExtensions
{
    public static string ToRialDisplay(this decimal? digit)
    {
        if (digit is null)
            return "";

        return Convert.ToInt32(digit).ToCurrencyStringFormat().En2FaDigits();
    }
}

