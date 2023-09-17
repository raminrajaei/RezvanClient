using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Pages.WorkHours.Models;
using ATA.HR.Shared.Dtos.CommitmentLetter;
using ATA.HR.Shared.Dtos.WorkHours;
using ATA.HR.Shared.Enums.Workflow;
using ATA.HR.Shared.Enums.WorkHours;
using ATABit.Helper.Extensions;
using ATABit.Helper.Utils;
using ATABit.Shared;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using DNTPersianUtils.Core;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.WorkHours;

[Authorize]
public partial class AllWorkHoursPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    public int TotalWorkHoursCountAtTargetMonth { get; set; }
    public int TotalEditedWorkHoursCountAtTargetMonth { get; set; }
    public int TotalWithAbsentCountAtTargetMonth { get; set; }
    public int TotalHaveNotSentWorkHoursCountAtTargetMonth { get; set; }
    public int TotalUsersUnregisteredAtTargetMonth { get; set; }
    private bool IsLoading { get; set; } = true;
    private string LoadingText { get; set; } = string.Empty;
    public List<string> AllUnits { get; set; } = new();
    public List<string> AllWorkLocations { get; set; } = new();
    private List<SelectListItem> WorkLocationsSource { get; set; } = new();
    private List<SelectListItem> RegistrarsSource { get; set; } = new();

    private List<SelectListItem> FlowsProgressSource { get; set; } = new()
    {
        new("در انتظار ارسال کارشناس منابع انسانی", WorkHourStateTag.StartWorkHourFlowByHR.ToString()),
        new("در انتظار امضای مدیر منابع انسانی", WorkHourStateTag.EditAndFinalizeWorkHourByHRSupervisor.ToString()),
        new("در انتظار امضای معاونت منابع انسانی", WorkHourStateTag.ConfirmWorkHourByHRManagement.ToString()),
        new("در انتظار امضای مدیر مستقیم مربوطه", WorkHourStateTag.ConfirmWorkHourByEmployeeDirectManager.ToString()),
        new("در انتظار امضای مدیر عامل", WorkHourStateTag.EditAndConfirmWorkHourByCEO.ToString()),
        new("پایان یافته", WorkHourStateTag.Finish.ToString())
    };

    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public string? UserNameSendingWorkHours { get; set; }
    public bool IsViewingRegisteredGrid { get; set; } = true;
    public bool IsViewingNotRegisteredGrid { get; set; }
    public bool IsViewingEditedGrid { get; set; }
    public bool IsViewingWithAbsentGrid { get; set; }
    public bool IsViewingFlightCrewEditForm => WorkHour.EmployeeWorkingTypeSelectedValue == EmployeeWorkingType.FlightCrew.ToString("D");
    public bool IsViewingSetadiEditForm => WorkHour.EmployeeWorkingTypeSelectedValue == EmployeeWorkingType.Setadi.ToString("D");
    public bool IsViewingHourlyEditForm => WorkHour.EmployeeWorkingTypeSelectedValue == EmployeeWorkingType.Hourly.ToString("D");

    public (int employeePersonnelCode, string employeeFullName, string employeePostTitle, string imageUrl) UserEmployeeDataToBeShownInEditForm { get; set; }

    public List<SelectListItem> EmployeeWorkingTypesSource { get; set; } = EnumMapping.ToSelectListItems<EmployeeWorkingType>();

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

    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    public bool IsVisibleMultipleActionButton { get; set; }
    private IEnumerable<WorkHourReadDto> SelectedItems { get; set; } = Enumerable.Empty<WorkHourReadDto>();

    private List<int> FlightCrewLastMonthPersonnelCodesOrdered { get; set; } = new();
    private List<int> NotFlightCrewLastMonthPersonnelCodesOrdered { get; set; } = new();

    #region Grid Filter Props
    // Rx
    private CancellationTokenSource _searchCancellationTokenSource = new();
    public WorkHoursFilterArgs WorkHoursFilter { get; set; } = new();
    public WorkHourDto WorkHour { get; set; } = new();
    // Rx (Reactive Extensions (Rx) framework)
    public Subject<string> SearchSubject { get; } = new();
    private bool IsFlightCrewMode => WorkHoursFilter.EmployeeWorkingTypeFilter == EmployeeWorkingType.FlightCrew;
    private bool IsSetadiMode => WorkHoursFilter.EmployeeWorkingTypeFilter == EmployeeWorkingType.Setadi;
    private bool IsHourlyMode => WorkHoursFilter.EmployeeWorkingTypeFilter == EmployeeWorkingType.Hourly;

    #endregion

    private List<SelectListItem> UnitsSource { get; set; } = new();

    public bool IsVisibleRegisterNewWorkHoursWindow { get; set; }
    public string? PersonnelCodeToRegisterNewWorkHours { get; set; }

    public bool IsSaveAndFinish { get; set; }
    public bool IsSaveAndContinue { get; set; }

    public bool IsVisibleDeleteWorkHoursConfirmDialog { get; set; }
    public int DeletingWorkHourId { get; set; }
    public string? DeletingWorkHourPersonnelName { get; set; }

    public bool IsVisibleAddNewWorkHourErrorMessageBox { get; set; }
    public string? AddNewWorkHourErrorMessage { get; set; }

    public bool IsVisibleAddNewWorkHourSuccessMessageBox { get; set; }
    public string? AddNewWorkHourSuccessMessage { get; set; }

    // Index
    private TelerikGrid<WorkHourReadDto> WorkHoursGridRef { get; set; }
    private TelerikGrid<UserDto> UnregisteredUsersGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
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
            // Rx
            TextSearchRxSubscribe();

            // Units
            AllUnits =  await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();

            // WorkLocations
            AllWorkLocations = await HttpClient.User().GetAllWorkLocations(cancellationToken: cancellationToken);

            WorkLocationsSource = AllWorkLocations.Select(wl => new SelectListItem(wl, wl)).ToList();

            await UpdateUsersWorkHoursStatistics(cancellationToken);

            FlightCrewLastMonthPersonnelCodesOrdered = await HttpClient.WorkHour().GetFlightCrewLastMonthPersonnelCodesOrdered(cancellationToken: cancellationToken);

            NotFlightCrewLastMonthPersonnelCodesOrdered = await HttpClient.WorkHour().GetNotFlightCrewLastMonthPersonnelCodesOrdered(cancellationToken: cancellationToken);

            // RegistrarsSource
            RegistrarsSource = await HttpClient.WorkHour().GetAllRegistrarsSource(cancellationToken: cancellationToken);

            await SetWorkHoursCountStatistics();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private async Task SetWorkHoursCountStatistics()
    {
        // WorkHours Count Statistics
        var stat = await HttpClient.WorkHour().GetWorkHoursCountStatistics(WorkHoursFilter);

        EmployeeWorkingTypesSource = EnumMapping.ToSelectListItems<EmployeeWorkingType>();

        StateHasChanged();

        foreach (var selectListItem in EmployeeWorkingTypesSource)
        {
            if (selectListItem.Value == EmployeeWorkingType.Setadi.ToString("D"))
                selectListItem.Text = $"{selectListItem.Text} [{stat.SetadiCount.ToPersianNumbers()}]";

            else if (selectListItem.Value == EmployeeWorkingType.FlightCrew.ToString("D"))
                selectListItem.Text = $"{selectListItem.Text} [{stat.FlightCrewCount.ToPersianNumbers()}]";

            else
                selectListItem.Text = $"{selectListItem.Text} [{stat.HourlyCount.ToPersianNumbers()}]";
        }
    }

    private void OnStateInitHandler(GridStateEventArgs<WorkHourReadDto> obj)
    {

    }

    private void OnStateInitHandler2(GridStateEventArgs<UserDto> obj)
    {

    }

    // Methods

    private async Task UpdateUsersWorkHoursStatistics(CancellationToken cancellationToken)
    {
        bool originalIsChanged = WorkHoursFilter.OnlyIsChanged;
        bool originalWithAbsentDay = WorkHoursFilter.OnlyWithAbsentDay;

        try
        {
            TotalUsersUnregisteredAtTargetMonth = await HttpClient.WorkHour().GetAllUnregisteredWorkHoursUsersCount(WorkHoursFilter, cancellationToken: cancellationToken);

            WorkHoursFilter.OnlyIsChanged = false;
            WorkHoursFilter.OnlyWithAbsentDay = false;
            TotalWorkHoursCountAtTargetMonth = await HttpClient.WorkHour().GetAllWorkHoursCount(WorkHoursFilter, cancellationToken: cancellationToken);

            WorkHoursFilter.OnlyIsChanged = true;
            WorkHoursFilter.OnlyWithAbsentDay = false;
            TotalEditedWorkHoursCountAtTargetMonth = await HttpClient.WorkHour().GetAllWorkHoursCount(WorkHoursFilter, cancellationToken: cancellationToken);

            WorkHoursFilter.OnlyWithAbsentDay = true;
            WorkHoursFilter.OnlyIsChanged = false;
            TotalWithAbsentCountAtTargetMonth = await HttpClient.WorkHour().GetAllWorkHoursCount(WorkHoursFilter, cancellationToken: cancellationToken);

            TotalHaveNotSentWorkHoursCountAtTargetMonth = await HttpClient.WorkHour().GetATAPersonnelSuitableToSendWorkHourCount(WorkHoursFilter, cancellationToken: cancellationToken);
        }
        catch
        {
            // Ignored
        }
        finally
        {
            WorkHoursFilter.OnlyIsChanged = originalIsChanged;
            WorkHoursFilter.OnlyWithAbsentDay = originalWithAbsentDay;

            StateHasChanged();
        }
    }

    protected async Task OnReadHandlerWorkHoursGrid(GridReadEventArgs args)
    {
        await LoadWorkHoursData(args);
    }

    protected async Task OnReadHandlerUnregisteredUsersGrid(GridReadEventArgs args)
    {
        await LoadUnregisteredUsersData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private async Task RebindGrids(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        try
        {
            ResetPagination = resetPagination;

            IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

            await UpdateUsersWorkHoursStatistics(CancellationToken.None);

            await WorkHoursGridRef.SetState(WorkHoursGridRef.GetState());

            if (UnregisteredUsersGridRef != null)
                await UnregisteredUsersGridRef.SetState(UnregisteredUsersGridRef.GetState());
        }
        catch
        {
            // Ignored
        }
        finally
        {
            StateHasChanged();
        }
    }

    private async Task LoadWorkHoursData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                WorkHoursGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            var data = await HttpClient.WorkHour().GetAllWorkHours(WorkHoursFilter, context, cancellationToken);

            data.ForEach(wh => wh.UserEmployeePicture = $"https://cdn.app.ataair.ir/img/pers/{wh.UserEmployeePersonnelCode}.png");

            args.Data = data;

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    private async Task LoadUnregisteredUsersData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                UnregisteredUsersGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            var data = await HttpClient.User().GetATAPersonnelUnregisteredWorkHours(WorkHoursFilter, context, cancellationToken);

            data.ForEach(u => u.FatherName = u.PictureURL);

            args.Data = data;

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public Task ApplyFilters => RebindGrids(true);

    public async Task ChangeYearOrMonthFilter()
    {
        await SetWorkHoursCountStatistics();

        await ApplyFilters;
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private void TextSearchRxSubscribe()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500)) // Ignore the requests coming before 500 milliseconds after previous request [Awesome Rx feature]
            .Where(t => t.IsNotNullOrEmpty() // Set condition for do subscribing using Linq [Great feature of Rx] 
                && (t!.Length > 2 || t.IsInt())
                || string.IsNullOrEmpty(t))
            .Subscribe(SearchObserver);

        // Local method. It could have been an inline lambda, but it's more elegant this way
        async void SearchObserver(string searchText)
        {
            // Cancel and dispose any previous token which already exists
            _searchCancellationTokenSource.Cancel();
            _searchCancellationTokenSource.Dispose();

            // Create new token source
            _searchCancellationTokenSource = new CancellationTokenSource();

            // Will fetch grid data again based on new entered search text
            await RebindGrids(true, true);
        }
    }

    public void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (WorkHoursFilter.SearchTerm == searchTerm) return; //Nothing has changed

        WorkHoursFilter.SearchTerm = searchTerm; //It'll be used in fetch data from DB 

        SearchSubject.OnNext(searchTerm); //Rx OnNext subscriber (observer) callback
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnUserImageLoadFailed()
    {
        UserEmployeeDataToBeShownInEditForm = (UserEmployeeDataToBeShownInEditForm.employeePersonnelCode,
            UserEmployeeDataToBeShownInEditForm.employeeFullName, UserEmployeeDataToBeShownInEditForm.employeePostTitle,
            "/images/layout/user.png");
    }

    private void OnUserImageLoadFailed(UserDto user)
    {
        user.FatherName = "/images/layout/user.png"; //Use Fathername instead of PicName because the latter doesn't have any setter
    }

    private void OnUserImageLoadFailed(WorkHourReadDto wh)
    {
        wh.UserEmployeePicture = "/images/layout/user.png"; //Use Fathername instead of PicName because the latter doesn't have any setter
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        //var workHour = (WorkHourReadDto)obj.Item;

        //if (workHour.LastContractEndDate is not null && workHour.LastContractEndDate.Value - DateTime.Now < TimeSpan.FromDays(30))
        //    obj.Class = "contract-ending-alert";
    }

    private void OnGridRowRender2(GridRowRenderEventArgs obj)
    {
        //var workHour = (WorkHourReadDto)obj.Item;

        //if (workHour.LastContractEndDate is not null && workHour.LastContractEndDate.Value - DateTime.Now < TimeSpan.FromDays(30))
        //    obj.Class = "contract-ending-alert";
    }

    private async Task ChangeToViewRegisteredPersonnelGrid()
    {
        if (WorkHoursFilter.OnlyIsChanged || WorkHoursFilter.OnlyWithAbsentDay)
        {
            WorkHoursFilter.OnlyIsChanged = false;

            WorkHoursFilter.OnlyWithAbsentDay = false;

            await ApplyFilters;
        }

        IsViewingRegisteredGrid = true;
        IsViewingEditedGrid = false;
        IsViewingNotRegisteredGrid = false;
        IsViewingWithAbsentGrid = false;
    }

    private void ChangeToViewNotRegisteredPersonnelGrid()
    {
        IsViewingRegisteredGrid = false;
        IsViewingEditedGrid = false;
        IsViewingNotRegisteredGrid = true;
        IsViewingWithAbsentGrid = false;
    }

    private async Task ChangeToViewEditedPersonnelGrid()
    {
        if (WorkHoursFilter.OnlyIsChanged is false || WorkHoursFilter.OnlyWithAbsentDay)
        {
            WorkHoursFilter.OnlyIsChanged = true;

            WorkHoursFilter.OnlyWithAbsentDay = false;

            await ApplyFilters;
        }

        IsViewingRegisteredGrid = false;
        IsViewingEditedGrid = true;
        IsViewingNotRegisteredGrid = false;
        IsViewingWithAbsentGrid = false;
    }

    private async Task ChangeToViewWithAbsentPersonnelGrid()
    {
        if (WorkHoursFilter.OnlyIsChanged || WorkHoursFilter.OnlyWithAbsentDay is false)
        {
            WorkHoursFilter.OnlyIsChanged = false;

            WorkHoursFilter.OnlyWithAbsentDay = true;

            await ApplyFilters;
        }

        IsViewingRegisteredGrid = false;
        IsViewingWithAbsentGrid = true;
        IsViewingEditedGrid = false;
        IsViewingNotRegisteredGrid = false;
    }

    private void SendAllFilteredRecords()
    {
        throw new NotImplementedException();
    }

    private async Task<DownloadFileResult> ExportWorkHoursDataToRahkaranExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            // Temporary disable EmployeeType in result
            var tempEmployeeWorkingType = WorkHoursFilter.EmployeeWorkingTypeSelectedValue;

            WorkHoursFilter.EmployeeWorkingTypeSelectedValue = null;

            var reportData = await HttpClient.WorkHour().GetAllWorkHours(WorkHoursFilter);

            if (reportData is null || reportData.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                reportData = reportData.OrderBy(wh => wh.UserEmployeePersonnelCode!.Value).ToList();

                WorkHoursFilter.EmployeeWorkingTypeSelectedValue = tempEmployeeWorkingType;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"کارکرد پرسنل {WorkHoursFilter.YearSelectedValue}-{WorkHoursFilter.MonthSelectedValue}")
                    .CreateGridLayoutExcel()
                    .WithOneSheetUsingModelBinding(reportData)
                    .Build();

                downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
            }
        }
        catch (Exception e)
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return downloadFileResult;
    }

    private async Task<DownloadFileResult> ExportWorkHoursDataToHRExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            // Temporary disable EmployeeType in result
            var tempEmployeeWorkingType = WorkHoursFilter.EmployeeWorkingTypeSelectedValue;

            WorkHoursFilter.EmployeeWorkingTypeSelectedValue = null;

            var reportData = await HttpClient.WorkHour().GetAllWorkHoursHRExcel(WorkHoursFilter);

            if (reportData is not null && IsMultiActionOnCustomSelection)
            {
                reportData = reportData.Where(e => SelectedItems.Any(wh => wh.Id == e.Id)).ToList();
            }

            if (reportData is null || reportData.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                reportData = reportData.OrderBy(wh => wh.UserEmployeePersonnelCode!.Value).ToList();

                WorkHoursFilter.EmployeeWorkingTypeSelectedValue = tempEmployeeWorkingType;

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName($"کارکرد پرسنل {WorkHoursFilter.YearSelectedValue}-{WorkHoursFilter.MonthSelectedValue}")
                    .CreateGridLayoutExcel()
                    .WithOneSheetUsingModelBinding(reportData)
                    .Build();

                downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
            }
        }
        catch (Exception e)
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return downloadFileResult;
    }

    private async Task<DownloadFileResult> ExportNotRegisteredPersonnelDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var reportData = await HttpClient.User().GetATAPersonnelUnregisteredWorkHours(WorkHoursFilter);

            if (reportData is null || reportData.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                var excelReportData = reportData.Select(u => new UnregisteredUser
                {
                    PersonnelCode = u.PersonnelCode,
                    PersonnelFullName = u.FullName,
                    PostTitle = u.PostTitle
                }).ToList();

                IExcelBuilder excelBuilder = ExcelBuilder
                    .SetGeneratedFileName("پرسنل ثبت نشده")
                    .CreateGridLayoutExcel()
                    .WithOneSheetUsingModelBinding(excelReportData)
                    .Build();

                downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
            }
        }
        catch (Exception e)
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return downloadFileResult;
    }

    private async Task OnWorkHourSubmit()
    {
        IsLoading = true;

        // RoutePlanning Logic. I didn't grasp it so I commented it out
        //if (PageOperationType is OperationType.Edit && IsSaveAndContinue)
        //    WorkHour.Id = null;

        try
        {
            var msg = "";

            if (PageOperationType is OperationType.Add or OperationType.Edit)
            {
                await HttpClient.WorkHour().SetWorkHour(WorkHour);

                var keyString = PageOperationType is OperationType.Add ? "ثبت " : "ویرایش";

                msg = $"کارکرد {UserEmployeeDataToBeShownInEditForm.employeeFullName} با موفقیت {keyString} شد";

                NotificationService.Toast(NotificationType.Success, msg);
            }

            bool operationOnFlightCrew = WorkHour.EmployeeWorkingTypeSelectedValue == EmployeeWorkingType.FlightCrew.ToString("D");

            if ((IsSaveAndContinue && PageOperationType is OperationType.Add) || (operationOnFlightCrew && PageOperationType is OperationType.Edit))
            {
                string? defaultPersonnelCode = null;

                if (operationOnFlightCrew)
                {
                    var currentUserIndex = FlightCrewLastMonthPersonnelCodesOrdered.IndexOf(UserEmployeeDataToBeShownInEditForm.employeePersonnelCode);

                    if (currentUserIndex != -1 && currentUserIndex != FlightCrewLastMonthPersonnelCodesOrdered.Count - 1)
                    {
                        defaultPersonnelCode = FlightCrewLastMonthPersonnelCodesOrdered[currentUserIndex + 1].ToString();
                    }
                }
                else
                {
                    var currentUserIndex = NotFlightCrewLastMonthPersonnelCodesOrdered.IndexOf(UserEmployeeDataToBeShownInEditForm.employeePersonnelCode);

                    if (currentUserIndex != -1 && currentUserIndex != NotFlightCrewLastMonthPersonnelCodesOrdered.Count - 1)
                    {
                        defaultPersonnelCode = NotFlightCrewLastMonthPersonnelCodesOrdered[currentUserIndex + 1].ToString();
                    }
                }

                await OpenRegisterNewWorkHourWindow(true, msg, defaultPersonnelCode);
            }

            else
            {
                ChangeToFilterMode();
            }

            await RebindGrids(false);
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            IsSaveAndContinue = false;
            IsSaveAndFinish = false;

            StateHasChanged();
        }
    }

    private async Task OpenWorkHourEditFormFor(UserDto user)
    {
        try
        {
            IsLoading = true;

            LoadingText = $"در حال دریافت اطلاعات کارکرد آخر {user.FullName}";

            PageOperationType = OperationType.Add;

            WorkHour = new WorkHourDto
            {
                UserIdEmployee = user.UserId,
                YearSelectedValue = WorkHoursFilter.YearSelectedValue,
                MonthSelectedValue = WorkHoursFilter.MonthSelectedValue
            };

            var lastWorkHours = await HttpClient.WorkHour().GetUserLastMonthWorkHours(user.UserId);

            if (lastWorkHours is not null && lastWorkHours.EmployeeWorkingType != default)
            {
                WorkHour.EmployeeWorkingTypeSelectedValue = lastWorkHours.EmployeeWorkingType.ToString();
                //WorkHour.Description = lastWorkHours.Description;
                //WorkHour.AttendantsFlightTimeHour = GetHoursFromTick(lastWorkHours.AttendantsFlightTime);
                //WorkHour.AttendantsFlightTimeMinute = GetMinutesFromTick(lastWorkHours.AttendantsFlightTime);
                //WorkHour.PilotFlightTimeHour = GetHoursFromTick(lastWorkHours.PilotFlightTime);
                //WorkHour.PilotFlightTimeMinute = GetMinutesFromTick(lastWorkHours.PilotFlightTime);
                //WorkHour.HoursOfStayOutsideTheCenterHour = GetHoursFromTick(lastWorkHours.HoursOfStayOutsideTheCenter);
                //WorkHour.HoursOfStayOutsideTheCenterMinute = GetMinutesFromTick(lastWorkHours.HoursOfStayOutsideTheCenter);
                //WorkHour.TechnicalFlightTimeHour = GetHoursFromTick(lastWorkHours.TechnicalFlightTime);
                //WorkHour.TechnicalFlightTimeMinute = GetMinutesFromTick(lastWorkHours.TechnicalFlightTime);
                //WorkHour.AttendantsFlightTimeHour = GetHoursFromTick(lastWorkHours.AttendantsFlightTime);
                //WorkHour.AttendantsFlightTimeMinute = GetMinutesFromTick(lastWorkHours.AttendantsFlightTime);
                //WorkHour.AttendantsHoursOfStayOutsideTheCenterHour = GetHoursFromTick(lastWorkHours.AttendantsHoursOfStayOutsideTheCenter);
                //WorkHour.AttendantsHoursOfStayOutsideTheCenterMinute = GetMinutesFromTick(lastWorkHours.AttendantsHoursOfStayOutsideTheCenter);
                //WorkHour.PilotPerDiem = lastWorkHours.PilotPerDiem;
                //WorkHour.AttendantsPerDiem = lastWorkHours.AttendantsPerDiem;
                //WorkHour.ExtraWorkTimeHour = GetHoursFromTick(lastWorkHours.ExtraWorkTime);
                //WorkHour.ExtraWorkTimeMinute = GetMinutesFromTick(lastWorkHours.ExtraWorkTime);
                //WorkHour.FridaysWorkTimeHour = GetHoursFromTick(lastWorkHours.FridaysWorkTime);
                //WorkHour.FridaysWorkTimeMinute = GetMinutesFromTick(lastWorkHours.FridaysWorkTime);
                //WorkHour.MonthlyWorkDeductionsHour = GetHoursFromTick(lastWorkHours.MonthlyWorkDeductions);
                //WorkHour.MonthlyWorkDeductionsMinute = GetMinutesFromTick(lastWorkHours.MonthlyWorkDeductions);
                WorkHour.HasShiftWork10Percent = lastWorkHours.HasShiftWork10Percent;
                WorkHour.HasShiftWork15Percent = lastWorkHours.HasShiftWork15Percent;
                WorkHour.HasShiftWork225Percent = lastWorkHours.HasShiftWork225Percent;
                //WorkHour.HourlyWorkTimeHour = GetHoursFromTick(lastWorkHours.HourlyWorkTime);
                //WorkHour.HourlyWorkTimeMinute = GetMinutesFromTick(lastWorkHours.HourlyWorkTime);
            }

            // Add user temporary data to be shown in EditForm
            UserEmployeeDataToBeShownInEditForm = (user.PersonnelCode, user.FullName, user.PostTitle, user.PictureURL);

            // Go to top of page (to EditForm)
            await JSRuntime.ScrollPageToTop();
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            LoadingText = string.Empty;

            StateHasChanged();
        }
    }

    private int? GetHoursFromTick(long tick)
    {
        var ts = new TimeSpan(tick);

        return Convert.ToInt32(Math.Floor(ts.TotalHours));
    }

    private int? GetMinutesFromTick(long tick)
    {
        var ts = new TimeSpan(tick);

        return Convert.ToInt32(ts.Minutes);
    }

    private void OnShit10PercentChanged(bool b)
    {
        if (b)
        {
            WorkHour.HasShiftWork15Percent = false;
            WorkHour.HasShiftWork225Percent = false;
        }
    }

    private void OnShit15PercentChanged(bool b)
    {
        if (b)
        {
            WorkHour.HasShiftWork10Percent = false;
            WorkHour.HasShiftWork225Percent = false;
        }
    }

    private void OnShit225PercentChanged(bool b)
    {
        if (b)
        {
            WorkHour.HasShiftWork10Percent = false;
            WorkHour.HasShiftWork15Percent = false;
        }
    }

    private async Task OpenRegisterNewWorkHourWindow(bool isAfterSaving = false, string msg = "", string defaultPersonnelCode = null)
    {
        PersonnelCodeToRegisterNewWorkHours = defaultPersonnelCode;

        IsVisibleRegisterNewWorkHoursWindow = true;

        IsVisibleAddNewWorkHourErrorMessageBox = false;

        AddNewWorkHourErrorMessage = "";

        IsVisibleAddNewWorkHourSuccessMessageBox = isAfterSaving;

        AddNewWorkHourSuccessMessage = msg;

        StateHasChanged();

        await Task.Delay(200);

        await JSRuntime.SetFocusAsync("personnelCodeToRegisterNewWorkHours");

        await Task.Delay(200);
    }

    private async Task PopulateEditFormForAddNewWorkHours()
    {
        IsVisibleAddNewWorkHourErrorMessageBox = false;
        AddNewWorkHourErrorMessage = "";

        IsVisibleAddNewWorkHourSuccessMessageBox = false;
        AddNewWorkHourSuccessMessage = "";

        LoadingText = "در حال بررسی پرسنل";

        IsLoading = true;

        StateHasChanged();

        // Check PersonnelCode not be empty and be a number
        if (string.IsNullOrWhiteSpace(PersonnelCodeToRegisterNewWorkHours))
        {
            var msg = "لطفا کد پرسنلی را وارد نمایید";
            IsVisibleAddNewWorkHourErrorMessageBox = true;
            AddNewWorkHourErrorMessage = msg;
            IsLoading = false;
            return;
        }

        if (PersonnelCodeToRegisterNewWorkHours.ToEnglishNumbers().IsInt() is false)
        {
            var msg = "فرمت شماره پرسنلی اشتباه است";
            IsVisibleAddNewWorkHourErrorMessageBox = true;
            AddNewWorkHourErrorMessage = msg;
            IsLoading = false;
            return;
        }

        // Check the user exists
        var user = await HttpClient.User().GetUserByPersonnelCode(PersonnelCodeToRegisterNewWorkHours.ToEnglishNumbers().ToInt());

        if (user is null || user.UserId == default)
        {
            var msg = "هیچ پرسنل فعالی با این کد پرسنلی پیدا نشد";
            IsVisibleAddNewWorkHourErrorMessageBox = true;
            AddNewWorkHourErrorMessage = msg;
            IsLoading = false;
            return;
        }

        // Check the user workhours already entered
        var userWorkHourForGivenMonth = await HttpClient.WorkHour().GetUserWorkHourIdForGivenMonth(user.UserId, WorkHoursFilter.YearSelectedValue!.ToInt(), WorkHoursFilter.MonthSelectedValue!.ToInt());

        if (userWorkHourForGivenMonth != null && userWorkHourForGivenMonth != 0 && PageOperationType == OperationType.Edit)
        {
            await ChangeToEditMode(userWorkHourForGivenMonth);

            IsVisibleRegisterNewWorkHoursWindow = false;

            return;
        }

        if (userWorkHourForGivenMonth != null && userWorkHourForGivenMonth != 0)
        {
            var msg = $"کارکرد {user.FullName} برای این ماه از قبل ثبت شده است";
            IsVisibleAddNewWorkHourErrorMessageBox = true;
            AddNewWorkHourErrorMessage = msg;
            IsLoading = false;
            return;
        }

        IsVisibleRegisterNewWorkHoursWindow = false;

        StateHasChanged();

        // Call the method OpenWorkHourEditFormFor (after closing the Modal)
        await OpenWorkHourEditFormFor(user);
    }

    private void SaveAndRegisterNew()
    {
        IsSaveAndContinue = true;

        // Save
        // Automatically with form submit
    }

    private void SaveAndReturn()
    {
        IsSaveAndFinish = true;

        // Save
        // Automatically with form submit
    }

    private void ShowDeleteWorkHourConfirmDialog(int whId)
    {
        DeletingWorkHourId = whId;

        IsVisibleDeleteWorkHoursConfirmDialog = true;
    }

    private async Task DeleteWorkHour(int deletingWorkHourId)
    {
        try
        {
            await HttpClient.WorkHour().DeleteWorkHour(new DeleteWorkHourArgs { WorkHourId = deletingWorkHourId });

            NotificationService.Toast(NotificationType.Success, "حذف ساعت کارکرد با موفقیت انجام شد");
        }
        catch
        {
            // ignored
        }
        finally
        {
            await RebindGrids(false);

            StateHasChanged();
        }
    }

    private async Task ChangeToEditMode(int whId)
    {
        IsLoading = true;

        try
        {
            WorkHour = await HttpClient.WorkHour().GetWorkHourByIdForForm(whId);

            var user = await HttpClient.User().GetUserById(WorkHour.UserIdEmployee);

            // Add user temporary data to be shown in EditForm
            UserEmployeeDataToBeShownInEditForm = (user.PersonnelCode, user.FullName, user.PostTitle, user.PictureURL);

            PageOperationType = OperationType.Edit;
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void ChangeEmployeeTypeInEditForm()
    {
        WorkHour.SetDefaultValuesByChangingEmployeeWorkingType();

        NotificationService.Toast(NotificationType.Warning, "با هر بار تغییر نوع پرسنل، کلیه ساعات ثبت شده پاک می‌شوند");
    }

    private async Task Enter(KeyboardEventArgs e)
    {
        if (e.Code == "Enter" || e.Code == "NumpadEnter")
        {
            await PopulateEditFormForAddNewWorkHours();
        }
    }

    private void GridSelectionModeChanged(object obj)
    {
        SelectedItems = new List<WorkHourReadDto>();
    }

    private async Task StartWorkHourFlow(int whId)
    {
        IsLoading = true;

        try
        {
            await HttpClient.WorkHour().StartWorkHourFlow(whId);

            NotificationService.Toast(NotificationType.Success, $"کارکرد با موفقیت  ارسال شد");

            await RebindGrids(false);
        }
        catch
        {
            // Ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SendAllFilteredWorkHours()
    {
        try
        {
            IsLoading = true;

            // Check not allowed checkboxes to send some message
            if (SelectedItems.Any(wh => wh.FlowCurrentStateTag.IsNotNullOrEmpty()))
            {
                int repCount = SelectedItems.Count(wh => wh.FlowCurrentStateTag.IsNotNullOrEmpty());

                NotificationService.Toast(NotificationType.Error, $"تعداد {repCount} پرسنل انتخاب شده تکراری می‌باشند و از قبل کارکردشان ارسال شده است");

                return;
            }

            var usersToSendWorkHours = await HttpClient.WorkHour().GetATAPersonnelSuitableToSendWorkHour(WorkHoursFilter);


            if (IsMultiActionOnCustomSelection)
            {
                usersToSendWorkHours = usersToSendWorkHours.Where(w => SelectedItems.Any(wh => wh.Id == w.WorkHourId)).ToList();
            }

            var allUsersCount = usersToSendWorkHours.Count;

            if (allUsersCount == 0)
            {
                NotificationService.Toast(NotificationType.Warning, $"هیچ پرسنلی برای ارسال کارکرد وجود ندارد");

                return;
            }

            int successCount = allUsersCount;

            foreach (var user in usersToSendWorkHours)
            {
                UserNameSendingWorkHours = user.FullName;

                StateHasChanged();

                try
                {
                    await HttpClient.WorkHour().StartWorkHourFlow(user.WorkHourId);
                }
                catch (Exception ex)
                {
                    successCount--;
                    await Task.Delay(2000);
                    // Ignored;
                }
            }

            if (successCount != 0)
                NotificationService.Toast(NotificationType.Success, $"تمامی {successCount} کارکرد با موفقیت ارسال شد");

            TotalHaveNotSentWorkHoursCountAtTargetMonth = await HttpClient.WorkHour().GetATAPersonnelSuitableToSendWorkHourCount(WorkHoursFilter);

            SelectedItems = Enumerable.Empty<WorkHourReadDto>();

            await RebindGrids(true);
        }
        catch
        {
            // ignored
        }
        finally
        {
            UserNameSendingWorkHours = "";

            IsLoading = false;

            StateHasChanged();
        }
    }

    private void OpenWorkHoursFlowFormPage(int whId)
    {
        NavigationManager.NavigateTo(PageUrls.WorkHoursFlowFormsPage(whId));
    }
}