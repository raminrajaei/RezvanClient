using ATABit.Helper.Extensions;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.CommitmentLetter;

[ComplexType]
[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "تعهدنامه‌های محضری")]
public class CommitmentLetterReadDto
{
    [ExcelSheetColumn(Ignore = true)]
    public int Id { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CreatedAt { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string CreatedAtJalali => CreatedAt.ToJalaliString();

    [ExcelSheetColumn(Ignore = true)]
    public int UserIdRegistrar { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? UserRegistrarFullName { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public int UserIdEmployee { get; set; }

    [ExcelSheetColumn(HeaderName = "نام پرسنل")]
    public string? UserEmployeeFullName { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "شماره پرسنلی")]
    public string? UserEmployeePersonnelCode { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "واحد")]
    public string? UserEmployeeUnitName { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "عنوان")]
    public string? Title { get; set; }

    [ExcelSheetColumn(HeaderName = "شماره تعهد محضری")]
    public string? CommitmentLetterNo { get; set; } //شماره تعهدنامه محضری

    [ExcelSheetColumn(Ignore = true)]
    public DateTime? CommitmentLetterRegisteredAt { get; set; } //تاریخ تعهد محضری

    [ExcelSheetColumn(HeaderName = "تاریخ تعهد محضری")]
    public string CommitmentLetterRegisteredAtJalali => CommitmentLetterRegisteredAt.HasValue ? CommitmentLetterRegisteredAt.Value.ToJalaliString() : "";

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CommitmentStartsAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ شروع تعهد")]
    public string CommitmentStartsAtJalali => CommitmentStartsAt.ToJalaliString();

    [ExcelSheetColumn(Ignore = true)]
    public DateTime CommitmentEndsAt { get; set; }

    [ExcelSheetColumn(HeaderName = "تاریخ پایان تعهد")]
    public string CommitmentEndsAtJalali => CommitmentEndsAt.ToJalaliString();

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan ValidityDuration => CommitmentEndsAt > DateTime.Now ? CommitmentEndsAt - DateTime.Now : TimeSpan.Zero;

    [ExcelSheetColumn(HeaderName = "مدت اعتبار")]
    public string ValidityDurationDisplay => ValidityDuration == TimeSpan.Zero ? "منقضی شده" : $"{Convert.ToInt32(ValidityDuration.TotalDays)} روز";
}