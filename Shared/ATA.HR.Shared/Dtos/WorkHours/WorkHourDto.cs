using ATA.HR.Shared.Enums.WorkHours;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours;

[ComplexType]
public class WorkHourDto : IValidatableObject
{
    public int? Id { get; set; }

    public int UserIdEmployee { get; set; }

    [Required(ErrorMessage = "سال کارکرد را انتخاب نمایید")]
    public string? YearSelectedValue { get; set; } =
        DateTime.Now.GetPersianYear().ToString(); // Iranian year number e.g. 1402

    [Required(ErrorMessage = "ماه کارکرد را انتخاب نمایید")]
    public string? MonthSelectedValue { get; set; } =
        DateTime.Now.GetPersianMonth().ToString(); // Iranian month number e.g. Ordibehesht = 2

    [Required(ErrorMessage = "نوع کارکرد را مشخص نمایید")]
    public string? EmployeeWorkingTypeSelectedValue { get; set; } = EmployeeWorkingType.Setadi.ToString("D");

    [MaxLength(500)] public string? Description { get; set; }

    public int WorkDays { get; set; } = GetDefaultWorkDays(); //روز کارکرد

    #region Flight Crew

    public int? PilotFlightTimeHour { get; set; } //ساعت پرواز خلبان
    public int? PilotFlightTimeMinute { get; set; } //ساعت پرواز خلبان
    public long PilotFlightTime => GetTicks(PilotFlightTimeHour, PilotFlightTimeMinute);

    public int? HoursOfStayOutsideTheCenterHour { get; set; } //ساعت اقامت خارج از مرکز
    public int? HoursOfStayOutsideTheCenterMinute { get; set; } //ساعت اقامت خارج از مرکز

    public long HoursOfStayOutsideTheCenter =>
        GetTicks(HoursOfStayOutsideTheCenterHour, HoursOfStayOutsideTheCenterMinute);

    public int? TechnicalFlightTimeHour { get; set; } //کارکرد پرواز فنی
    public int? TechnicalFlightTimeMinute { get; set; } //کارکرد پرواز فنی
    public long TechnicalFlightTime => GetTicks(TechnicalFlightTimeHour, TechnicalFlightTimeMinute);

    public int? AttendantsFlightTimeHour { get; set; } //ساعت پرواز مهمانداران
    public int? AttendantsFlightTimeMinute { get; set; } //ساعت پرواز مهمانداران
    public long AttendantsFlightTime => GetTicks(AttendantsFlightTimeHour, AttendantsFlightTimeMinute);

    public int? AttendantsHoursOfStayOutsideTheCenterHour { get; set; } //ساعت اقامت خارج از مرکز مهمانداران
    public int? AttendantsHoursOfStayOutsideTheCenterMinute { get; set; } //ساعت اقامت خارج از مرکز مهمانداران

    public long AttendantsHoursOfStayOutsideTheCenter => GetTicks(AttendantsHoursOfStayOutsideTheCenterHour,
        AttendantsHoursOfStayOutsideTheCenterMinute);

    public int PilotPerDiem { get; set; } //حق پردیوم خلبان و کمک خلبان - روز

    public int AttendantsPerDiem { get; set; } //حق پردیوم سرمهماندار و مهماندار - روز


    #endregion

    public int? ExtraWorkTimeHour { get; set; } //کارکرد اضافه کاری
    public int? ExtraWorkTimeMinute { get; set; } //کارکرد اضافه کاری
    public long ExtraWorkTime => GetTicks(ExtraWorkTimeHour, ExtraWorkTimeMinute);

    #region Setadi

    public int? FridaysWorkTimeHour { get; set; } //جمعه کاری
    public int? FridaysWorkTimeMinute { get; set; } //جمعه کاری
    public long FridaysWorkTime => GetTicks(FridaysWorkTimeHour, FridaysWorkTimeMinute);

    public int? MonthlyWorkDeductionsHour { get; set; } //کسر کار ماهانه
    public int? MonthlyWorkDeductionsMinute { get; set; } //کسر کار ماهانه
    public long MonthlyWorkDeductions => GetTicks(MonthlyWorkDeductionsHour, MonthlyWorkDeductionsMinute);

    public bool HasShiftWork10Percent { get; set; } //کارکرد نوبت کاری 10%

    public bool HasShiftWork15Percent { get; set; } //کارکرد شیفت 15%

    public bool HasShiftWork225Percent { get; set; } //کارکرد شیفت 22.5%

    public int? HourlyWorkTimeHour { get; set; } //کارکرد ساعتی
    public int? HourlyWorkTimeMinute { get; set; } //کارکرد ساعتی
    public long HourlyWorkTime => GetTicks(HourlyWorkTimeHour, HourlyWorkTimeMinute);

    #endregion

    // Methods
    private long GetTicks(int? hours, int? minutes)
    {
        hours ??= 0;
        minutes ??= 0;

        var timeSpan = TimeSpan.FromHours(hours.Value) + TimeSpan.FromMinutes(minutes.Value);

        return timeSpan.Ticks;
    }

    private static int GetDefaultWorkDays()
    {
        if (DateTime.Now.GetPersianMonth() == 12)
            return 29;

        if (DateTime.Now.GetPersianMonth() < 7)
            return 31;

        return 30;
    }

    public void SetDefaultValuesByChangingEmployeeWorkingType()
    {
        // Flight Crew 

        PilotFlightTimeHour = default;
        PilotFlightTimeMinute = default;

        HoursOfStayOutsideTheCenterHour = default;
        HoursOfStayOutsideTheCenterMinute = default;

        TechnicalFlightTimeHour = default;
        TechnicalFlightTimeMinute = default;

        AttendantsFlightTimeHour = default;
        AttendantsFlightTimeMinute = default;

        AttendantsHoursOfStayOutsideTheCenterHour = default;
        AttendantsHoursOfStayOutsideTheCenterMinute = default;

        PilotPerDiem = default;
        AttendantsPerDiem = default;

        // Common

        ExtraWorkTimeHour = default;
        ExtraWorkTimeMinute = default;

        // Setadi

        FridaysWorkTimeHour = default;
        FridaysWorkTimeMinute = default;

        MonthlyWorkDeductionsHour = default;
        MonthlyWorkDeductionsMinute = default;

        HasShiftWork10Percent = default;
        HasShiftWork15Percent = default;
        HasShiftWork225Percent = default;

        HourlyWorkTimeHour = default;
        HourlyWorkTimeMinute = default;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // سقف کارکرد 31 روز
        if (WorkDays > 31)
            yield return new ValidationResult("سقف کارکرد 31 روز می‌باشد", new List<string> { nameof(WorkDays) });

        // اضافه کاری و جمعه کاری 120 ساعت
        //if (ExtraWorkTime > TimeSpan.FromHours(120).Ticks)
        //    yield return new ValidationResult("حداکثر اضافه کاری 120 ساعت می‌باشد", new List<string> { nameof(ExtraWorkTimeHour) });

        if (FridaysWorkTime > TimeSpan.FromHours(120).Ticks)
            yield return new ValidationResult("حداکثر جمعه کاری 120 ساعت می‌باشد", new List<string> { nameof(FridaysWorkTimeHour) });

        // ساعت پرواز خلبان 90 
        if (PilotFlightTime > TimeSpan.FromHours(90).Ticks)
            yield return new ValidationResult("حداکثر ساعت پرواز خلبانان 90 ساعت می‌باشد", new List<string> { nameof(PilotFlightTimeHour) });

        // ساعت پرواز مهماندار 90 
        if (AttendantsFlightTime > TimeSpan.FromHours(90).Ticks)
            yield return new ValidationResult("حداکثر ساعت پرواز مهمانداران 90 ساعت می‌باشد", new List<string> { nameof(AttendantsFlightTimeHour) });
    }
}