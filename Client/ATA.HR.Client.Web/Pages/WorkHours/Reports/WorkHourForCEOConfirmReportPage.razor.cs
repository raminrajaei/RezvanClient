using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Dtos.WorkHours;
using ATA.HR.Shared.Dtos.WorkHours.Reports;
using ATABit.Shared;
using Bit.Utils.Extensions;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading;
using Telerik.DataSource.Extensions;

namespace ATA.HR.Client.Web.Pages.WorkHours.Reports;

public partial class WorkHourForCEOConfirmReportPage
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    private bool ShowResult { get; set; }
    public List<SelectListItem> ConfirmersSource { get; set; } = new();
    public List<DirectManagerReadDto> DirectManagers { get; set; } = new();

    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public List<List<WorkHourReadDto>> WorkHoursDataList { get; set; } = new();
    public WorkHourReadDto? WorkHourSignaturesRef => WorkHoursDataList.FirstOrDefault()?.FirstOrDefault();
    public WorkHourForCeoConfirmFilterArgs GetDataArgs { get; set; } = new();
    public string? ManagerComment { get; set; }

    public string? UnitName => GetUnitName();
    public (string? CurrentMonthStart, string? CurrentMonthFinish) CurrentMonth => GetCurrentMonthStartAndFinish();

    public (string? PreviousMonthStart, string? PreviousMonthFinish) PreviousMonth => GetPreviousMonthStartAndFinish();

    public List<SelectListItem> YearsSource { get; set; } = new()
    {
        new("1401".ToPersianNumbers(), "1401"),
        new("1402".ToPersianNumbers(), "1402"),
        new("1403".ToPersianNumbers(), "1403"),
        new("1404".ToPersianNumbers(), "1404")
    };
    public List<SelectListItem> MonthsSource { get; set; } = new()
    {
        new("فروردین", "1"),
        new("اردیبهشت", "2"),
        new("خرداد", "3"),
        new("تیر", "4"),
        new("مرداد", "5"),
        new("شهریور", "6"),
        new("مهر", "7"),
        new("آبان", "8"),
        new("آذر", "9"),
        new( "دی", "10"),
        new( "بهمن", "11"),
        new( "اسفند", "12")
    };

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            // DirectManagers
            DirectManagers = await HttpClient.User().WorkHoursGetDirectManagers(cancellationToken: cancellationToken);

            ConfirmersSource = DirectManagers.Select(m => new SelectListItem($"{m.UnitName} ({m.ConfirmerFullName})", m.ConfirmerUserId.ToString())).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private async Task GetReport()
    {
        IsLoading = true;

        ManagerComment = "";

        try
        {
            ManagerComment = await HttpClient.WorkHour().GetDirectManagerCommentOnWorkHours(GetDataArgs);

            var workHoursData = await HttpClient.WorkHour().GetReportDataForCeoConfirm(GetDataArgs);

            var i = 1;
            workHoursData.ForEach(wh =>
            {
                wh.Id = i;
                i++;
            });

            WorkHoursDataList = PaginateData(workHoursData);

            ShowResult = true;
        }
        catch
        {
            // Ignored

            ShowResult = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    private List<List<WorkHourReadDto>> PaginateData(List<WorkHourReadDto> data)
    {
        List<List<WorkHourReadDto>> result = new();

        bool go = true;

        int currentIndex = 0;

        int remainedSize = data.Count;

        while (go)
        {
            int i = 18;

            if (remainedSize is >= 16 and <= 18)
            {
                i = 15;
            }

            var pageData = data.Skip(currentIndex).Take(i).ToList();

            result.Add(pageData);
            currentIndex += i;

            remainedSize -= i;

            if (currentIndex > data.Count)
                go = false;
        }

        return result;
    }

    private string? GetUnitName()
    {
        if (GetDataArgs.ConfirmerUserIdSelectedValue.IsNotNullOrEmpty())
        {
            return DirectManagers
                .SingleOrDefault(m => m.ConfirmerUserId == GetDataArgs.ConfirmerUserIdSelectedValue!.ToInt())
                ?.UnitName;
        }

        return null;
    }

    private (string? CurrentMonthStart, string? CurrentMonthFinish) GetCurrentMonthStartAndFinish()
    {
        if (GetDataArgs.YearSelectedValue.HasValue())
        {
            var startAndEnd = GetDataArgs.YearSelectedValue!.ToInt().GetPersianMonthStartAndEndDates(GetDataArgs.MonthSelectedValue!.ToInt());

            return (startAndEnd.StartDate.ToJalaliString(), startAndEnd.EndDate.ToJalaliString());
        }

        return (null, null);
    }

    private (string? PreviousMonthStart, string? PreviousMonthFinish) GetPreviousMonthStartAndFinish()
    {
        if (GetDataArgs.YearSelectedValue.HasValue())
        {
            var currentMonth = $"{GetDataArgs.YearSelectedValue}/{GetDataArgs.MonthSelectedValue}/05".ToDateTime();
            var previousMonth = currentMonth.AddMonths(-1);

            var startAndEnd = previousMonth.GetPersianYear().GetPersianMonthStartAndEndDates(previousMonth.GetPersianMonth());

            return (startAndEnd.StartDate.ToJalaliString(), startAndEnd.EndDate.ToJalaliString());
        }

        return (null, null);
    }
}

