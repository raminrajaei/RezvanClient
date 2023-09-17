using ATA.HR.Shared.Enums.WorkHours;
using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using ExcelWizard.Models;
using ExcelWizard.Models.EWGridLayout;
using ExcelWizard.Models.EWStyles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours;

// Sync this class with WorkHourHRExcelReadDto
[ComplexType]
[ExcelSheet(SheetDirection = SheetDirection.RightToLeft, SheetName = "کارکرد پرسنل", BorderType = LineStyle.Thin,
    IsSheetHardProtected = false, IsSheetLocked = false, DataRowHeight = 20, HeaderHeight = 20)]
public class WorkHourReadDto
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

    [ExcelSheetColumn(Ignore = true)]
    public string? UserEmployeeFullName { get; set; } //Flattening

    [ExcelSheetColumn(HeaderName = "کد پرسنلی", ExcelDataContentType = CellContentType.General, ColumnWidth = 12)]
    public int? UserEmployeePersonnelCode { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public string? UserEmployeePicture { get; set; } //Will populate in client

    [ExcelSheetColumn(Ignore = true)]
    public string UserEmployeePersonnelCodeToDisplay => UserEmployeePersonnelCode.ToPersianNumbers();

    [ExcelSheetColumn(Ignore = true)]
    public string? UserEmployeeFirstName { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public string? UserEmployeeLastName { get; set; } //Flattening

    [ExcelSheetColumn(Ignore = true)]
    public int Year { get; set; } // Iranian year number e.g. 1402

    [ExcelSheetColumn(Ignore = true)]
    public int Month { get; set; } // Iranian month number e.g. Ordibehesht = 2

    [ExcelSheetColumn(Ignore = true)]
    public string RefDateDisplay => $"{Month.GetPersianMonthName()} {Year.ToPersianNumbers()}";

    [ExcelSheetColumn(Ignore = true)]
    public int EmployeeWorkingType { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public string? EmployeeWorkingTypeDisplay => ((EmployeeWorkingType)EmployeeWorkingType).ToDisplayName();

    [MaxLength(500)]
    [ExcelSheetColumn(Ignore = true)]
    public string? Description { get; set; }

    [ExcelSheetColumn(Ignore = true)]
    public int WorkDays { get; set; } //روز کارکرد

    [ExcelSheetColumn(Ignore = true)]
    public string AbsentDays => (GetDefaultWorkDays() - WorkDays).ToPersianNumbers(); //روز غیبت

    [ExcelSheetColumn(Ignore = true)]
    public string WorkDaysToDisplay => WorkDays.ToPersianNumbers();

    #region Flight Crew

    [ExcelSheetColumn(Ignore = true)]
    public long PilotFlightTime { get; set; } //ساعت پرواز خلبان

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan PilotFlightTimeSpan => new(PilotFlightTime);

    [ExcelSheetColumn(Ignore = true)]
    public string PilotFlightTimeDisplay => TimeSpanToDisplay(PilotFlightTimeSpan);

    [ExcelSheetColumn(HeaderName = "ساعت پرواز خلبان", ColumnWidth = 15)]
    public string PilotFlightTimeExcelDisplay => TimeSpanToExcelDisplay(PilotFlightTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public long HoursOfStayOutsideTheCenter { get; set; } //ساعت اقامت خارج از مرکز

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan HoursOfStayOutsideTheCenterTimeSpan => new(HoursOfStayOutsideTheCenter);

    [ExcelSheetColumn(Ignore = true)]
    public string HoursOfStayOutsideTheCenterTimeDisplay => TimeSpanToDisplay(HoursOfStayOutsideTheCenterTimeSpan);

    [ExcelSheetColumn(HeaderName = "ساعت اقامت خارج از مرکز", ExcelDataContentType = CellContentType.Text, ColumnWidth = 20)]
    public string HoursOfStayOutsideTheCenterTimeExcelDisplay => TimeSpanToExcelDisplay(HoursOfStayOutsideTheCenterTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public long TechnicalFlightTime { get; set; } //کارکرد پرواز فنی

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan TechnicalFlightTimeSpan => new(TechnicalFlightTime);

    [ExcelSheetColumn(Ignore = true)]
    public string TechnicalFlightTimeDisplay => TimeSpanToDisplay(TechnicalFlightTimeSpan);

    [ExcelSheetColumn(HeaderName = "کارکرد پرواز فنی", ExcelDataContentType = CellContentType.Text, ColumnWidth = 13)]
    public string TechnicalFlightTimeExcelDisplay => TimeSpanToExcelDisplay(TechnicalFlightTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public long AttendantsFlightTime { get; set; } //ساعت پرواز مهمانداران

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan AttendantsFlightTimeSpan => new(AttendantsFlightTime);

    [ExcelSheetColumn(Ignore = true)]
    public string AttendantsFlightTimeDisplay => TimeSpanToDisplay(AttendantsFlightTimeSpan);

    [ExcelSheetColumn(HeaderName = "ساعت پروازمهمانداران", ExcelDataContentType = CellContentType.Text, ColumnWidth = 17)]
    public string AttendantsFlightTimeExcelDisplay => TimeSpanToExcelDisplay(AttendantsFlightTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public long AttendantsHoursOfStayOutsideTheCenter { get; set; } //ساعت اقامت خارج از مرکز مهمانداران

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan AttendantsHoursOfStayOutsideTheCenterTimeSpan => new(AttendantsHoursOfStayOutsideTheCenter);

    [ExcelSheetColumn(Ignore = true)]
    public string AttendantsHoursOfStayOutsideTheCenterTimeDisplay => TimeSpanToDisplay(AttendantsHoursOfStayOutsideTheCenterTimeSpan);

    [ExcelSheetColumn(HeaderName = "ساعت اقامت خارج از مرکز مهمانداران", ExcelDataContentType = CellContentType.Text, ColumnWidth = 27)]
    public string AttendantsHoursOfStayOutsideTheCenterTimeExcelDisplay => TimeSpanToExcelDisplay(AttendantsHoursOfStayOutsideTheCenterTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public int PilotPerDiem { get; set; } //حق پردیوم خلبان و کمک خلبان - روز

    [ExcelSheetColumn(HeaderName = "حق پردیوم خلبان و کمک خلبان", ExcelDataContentType = CellContentType.General, ColumnWidth = 22)]
    public string PilotPerDiemToDisplay => PilotPerDiem.ToString().ToPersianNumbers();

    [ExcelSheetColumn(Ignore = true)]
    public int AttendantsPerDiem { get; set; } //حق پردیوم سرمهماندار و مهماندار - روز

    [ExcelSheetColumn(HeaderName = "حق پردیوم سرمهماندارومهماندار", ExcelDataContentType = CellContentType.General, ColumnWidth = 22)]
    public string AttendantsPerDiemToDisplay => AttendantsPerDiem.ToString().ToPersianNumbers();
    #endregion

    [ExcelSheetColumn(Ignore = true)]
    public long ExtraWorkTime { get; set; } //کارکرد اضافه کاری

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan ExtraWorkTimeSpan => new(ExtraWorkTime);

    [ExcelSheetColumn(Ignore = true)]
    public string ExtraWorkTimeDisplay => TimeSpanToDisplay(ExtraWorkTimeSpan);

    [ExcelSheetColumn(HeaderName = "کارکرد اضافه کاری", ExcelDataContentType = CellContentType.Text, ColumnWidth = 14)]
    public string ExtraWorkTimeExcelDisplay => TimeSpanToExcelDisplay(ExtraWorkTimeSpan);

    #region Setadi

    [ExcelSheetColumn(Ignore = true)]
    public long FridaysWorkTime { get; set; } //جمعه کاری

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan FridaysWorkTimeSpan => new(FridaysWorkTime);

    [ExcelSheetColumn(Ignore = true)]
    public string FridaysWorkTimeDisplay => TimeSpanToDisplay(FridaysWorkTimeSpan);

    [ExcelSheetColumn(HeaderName = "جمعه کاری", ExcelDataContentType = CellContentType.Text, ColumnWidth = 12)]
    public string FridaysWorkTimeExcelDisplay => TimeSpanToExcelDisplay(FridaysWorkTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public long MonthlyWorkDeductions { get; set; } //کسر کار ماهانه

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan MonthlyWorkDeductionsTimeSpan => new(MonthlyWorkDeductions);

    [ExcelSheetColumn(Ignore = true)]
    public string MonthlyWorkDeductionsTimeDisplay => TimeSpanToDisplay(MonthlyWorkDeductionsTimeSpan);

    [ExcelSheetColumn(HeaderName = "کسرکار ماهانه", ExcelDataContentType = CellContentType.Text, ColumnWidth = 12)]
    public string MonthlyWorkDeductionsTimeExcelDisplay => TimeSpanToExcelDisplay(MonthlyWorkDeductionsTimeSpan);

    [ExcelSheetColumn(Ignore = true)]
    public bool HasShiftWork10Percent { get; set; } //کارکرد نوبت کاری 10%

    [ExcelSheetColumn(HeaderName = "کارکرد نوبتکاری 10%", ExcelDataContentType = CellContentType.General, ColumnWidth = 16)]
    public string HasShiftWork10PercentExcelDisplay => HasShiftWork10Percent ? "1" : "0";

    [ExcelSheetColumn(Ignore = true)]
    public bool HasShiftWork15Percent { get; set; } //کارکرد شیفت 15%

    [ExcelSheetColumn(HeaderName = "کارکرد شیفت 15%", ExcelDataContentType = CellContentType.General, ColumnWidth = 16)]
    public string HasShiftWork15PercentExcelDisplay => HasShiftWork15Percent ? "1" : "0";

    [ExcelSheetColumn(Ignore = true)]
    public bool HasShiftWork225Percent { get; set; } //کارکرد شیفت 22.5%

    [ExcelSheetColumn(HeaderName = "کارکرد شیفت 22.5%", ExcelDataContentType = CellContentType.General, ColumnWidth = 16)]
    public string HasShiftWork225PercentExcelDisplay => HasShiftWork225Percent ? "1" : "0";

    [ExcelSheetColumn(Ignore = true)]
    public string WorkShiftGridDisplay =>
        HasShiftWork10Percent ? "10%".ToPersianNumbers()
        : HasShiftWork15Percent ? "15%".ToPersianNumbers()
        : HasShiftWork225Percent ? "22.5%".ToPersianNumbers()
        : "0".ToPersianNumbers();

    [ExcelSheetColumn(Ignore = true)]
    public long HourlyWorkTime { get; set; } //کارکرد ساعتی

    [ExcelSheetColumn(Ignore = true)]
    public TimeSpan HourlyWorkTimeSpan => new(HourlyWorkTime);

    [ExcelSheetColumn(Ignore = true)]
    public string HourlyWorkTimeDisplay => TimeSpanToDisplay(HourlyWorkTimeSpan);

    [ExcelSheetColumn(HeaderName = "کارکرد ساعتی", ExcelDataContentType = CellContentType.Text, ColumnWidth = 12)]
    public string HourlyWorkTimeExcelDisplay => TimeSpanToExcelDisplay(HourlyWorkTimeSpan);

    #endregion

    [ExcelSheetColumn(Ignore = true)]
    public string? UserEmployeeUnitName { get; set; } //Flattening

    #region Signatures

    public string? HRSeniorExpertImageDataURL { get; set; }
    public string? HRSeniorExpertSignedBy { get; set; }

    public string? HRSupervisorSignatureImageDataURL { get; set; }
    public string? HRSupervisorSignedBy { get; set; }

    public string? HRManagerSignatureImageDataURL { get; set; }
    public string? HRManagerSignedBy { get; set; }

    public string? DirectManagerSignatureImageDataURL { get; set; }
    public string? DirectManagerSignedBy { get; set; }

    public string? CEOSignatureImageDataURL { get; set; }
    public string? CEOSignedBy { get; set; }

    #endregion

    public int? FlowGeneralStatus { get; set; }

    public string? FlowCurrentStateTag { get; set; }

    public string? CurrentStateTagFromWorkflow { get; set; }

    // Private Methods
    private string TimeSpanToDisplay(TimeSpan time) => $"{Convert.ToInt32(Math.Floor(time.TotalHours)):000}:{time.Minutes:00}".ToPersianNumbers();
    private string TimeSpanToExcelDisplay(TimeSpan time) => $"{Convert.ToInt32(Math.Floor(time.TotalHours)):000}:{time.Minutes:00}";
    private int GetDefaultWorkDays()
    {
        if (EmployeeWorkingType == (int)Enums.WorkHours.EmployeeWorkingType.Hourly)
            return 0;

        // TODO: Calculate Kabise Year

        // Esfand
        if (Month == 12)
            return 29;

        // Spring & Summer
        if (Month < 7)
            return 31;

        // Winter & Autumn
        return 30;
    }
}