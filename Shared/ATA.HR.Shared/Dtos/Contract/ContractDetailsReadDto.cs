using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class ContractDetailsReadDto : ContractDetailsBase
{
    public int ContractId { get; set; }

    public string? FullName { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UserAddress { get; set; }

    public string? ATACEOName { get; set; } //For Hourly Contracts

    public string? ATAAddress { get; set; } //For Hourly Contracts

    // Calculated Props
    public decimal? TargetJobWage => CalculateJobWage(JobWage, BaseWage, YearConstantIncrease, ExcellentQualification);
    public decimal? DailyWage => TargetJobWage / 30;
    public decimal? DailyYearsBasePay => YearsBasePay / 30;
    public string WorkDurationFriendlyDisplay => RelativeTimeCalculator.Calculate(EmploymentDate!.ToDateTime(), CreatedAt);

    // Method
    private decimal? CalculateJobWage(decimal? jobWage, decimal? baseWage, decimal? yearConstantIncrease, decimal? excellentQualification)
    {
        if (jobWage.HasValue && jobWage != 0)
            return jobWage.Value;

        if (baseWage.HasValue && yearConstantIncrease.HasValue && excellentQualification.HasValue)
            return BaseWage!.Value + YearConstantIncrease!.Value + ExcellentQualification!.Value;

        return null;
    }
}

public class RelativeTimeCalculator
{
    private const int SECOND = 1;
    private const int MINUTE = 60 * SECOND;
    private const int HOUR = 60 * MINUTE;
    private const int DAY = 24 * HOUR;
    private const int MONTH = 30 * DAY;

    public static string Calculate(DateTime employmentDateTime, DateTime contractRegisterDateTime)
    {
        var ts = new TimeSpan(contractRegisterDateTime.Ticks - employmentDateTime.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);
        if (delta < 1 * MINUTE)
        {
            return ts.Seconds == 1 ? "لحظه ای قبل" : ts.Seconds + " ثانیه قبل";
        }
        if (delta < 2 * MINUTE)
        {
            return "یک دقیقه قبل";
        }
        if (delta < 45 * MINUTE)
        {
            return ts.Minutes + " دقیقه قبل";
        }
        if (delta < 90 * MINUTE)
        {
            return "یک ساعت قبل";
        }
        if (delta < 24 * HOUR)
        {
            return ts.Hours + " ساعت قبل";
        }
        if (delta < 48 * HOUR)
        {
            return "دیروز";
        }
        if (delta < 30 * DAY)
        {
            return ts.Days + " روز قبل";
        }
        if (delta < 12 * MONTH)
        {
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "یک ماه " : months + " ماه";
        }

        int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));

        var tsMin = new TimeSpan(contractRegisterDateTime.Ticks - employmentDateTime.AddYears(years).Ticks);

        int month = Convert.ToInt32(Math.Floor((double)tsMin.Days / 30));

        var tsDay = new TimeSpan(contractRegisterDateTime.Ticks - employmentDateTime.AddYears(years).AddMonths(month).Ticks);

        return $"{years} سال و {month} ماه و {tsDay.Days} روز";
    }

}
