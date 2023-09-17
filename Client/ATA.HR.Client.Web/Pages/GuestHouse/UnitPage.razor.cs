using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using Bit.Http.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.GuestHouse;

[Authorize]
public partial class UnitPage : IDisposable
{
    // Consts
    private const int PageSize = 40;

    private UnitReadDto SelectedUnit;

    // Props
    public int TotalUnitCounts { get; set; }
    public bool IsChangeUnitStatusWindowVisible { get; set; } = false;
    public bool SelectedUnitStatus { get; set; }
    public bool IsLoading { get; set; } = true;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public OperationType PageOperationType { get; set; } = OperationType.Filter;

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public UnitFilterArgs UnitDataFilter { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    // Index
    private TelerikGrid<UnitReadDto> UnitGridRef { get; set; }
    private List<UnitReadDto> UnitList { get; set; } = new();
    public UnitDto Unit { get; set; } = new();
    public string City { get; set; }
    public string CityName { get; set; }
    public string BuildingName { get; set; }

    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public int BuildingId { get; set; }

    // Life Cycles
    protected async override Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            UnitDataFilter.BuildingId = Unit.BuildingId = BuildingId;

            var buildingInfo = await HttpClient.Building().GetById(BuildingId, cancellationToken: cancellationToken);

            if (buildingInfo != null)
            {
                var city = (ReferenceCity)buildingInfo.CityId;

                City = city.ToString();
                CityName = city.ToDisplayName();
                BuildingName = buildingInfo.Title;
            }

            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    // Methods
    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        return UnitGridRef.SetState(UnitGridRef.GetState());
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;
    private void ChangeToAddUnitMode() => PageOperationType = OperationType.Add;
    public Task ApplyFilters => RebindGrid(true);

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                UnitGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            args.Data = UnitList = await HttpClient.Unit().GetUnitBuildings(UnitDataFilter, context);

            args.Total = TotalUnitCounts = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public async Task OnUnitSubmit()
    {
        try
        {
            await HttpClient.Unit().AddUnit(Unit);

            NotificationService.Toast(NotificationType.Success, $"واحد {Unit.Title} در ساختمان {BuildingName} با موفقیت ذخیره گردید.");

            ChangeToFilterMode();
            Unit = new();

            await RebindGrid(false);
        }
        catch (Exception exp)
        {
            ExceptionHandler.OnExceptionReceived(exp);
        }
    }

    private void SearchSubscriber()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(t => t.IsNotNullOrEmpty() && t!.Length > 2 || string.IsNullOrEmpty(t))
            .Subscribe(async _ =>
            {
                try
                {
                    IsLoading = true;

                    _searchCancellationTokenSource.Cancel();

                    _searchCancellationTokenSource.Dispose();

                    _searchCancellationTokenSource = new CancellationTokenSource();

                    await RebindGrid(true, true);
                }
                catch (Exception exp)
                {
                    ExceptionHandler.OnExceptionReceived(exp);
                }
                finally
                {
                    StateHasChanged();
                }
            });
    }

    public void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (UnitDataFilter.SearchTerm != searchTerm)
            UnitDataFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(UnitDataFilter.SearchTerm);
    }

    public void OpenRoomPage(int unitId)
    {
        if (unitId == 0) return;

        NavigationManager.NavigateTo(PageUrls.RoomPage(BuildingId, unitId));
    }

    public void ChangeUnitStatus(UnitReadDto unit)
    {
        if (unit == null) return;

        SelectedUnit = unit;
        IsChangeUnitStatusWindowVisible = true;

        SelectedUnitStatus = UnitList.FirstOrDefault(u => u.Id == unit.Id).IsActive;
    }

    public async Task ConfirmChangeUnitStatus()
    {
        SelectedUnit.IsActive = !SelectedUnitStatus;

        await HttpClient.Unit().UpdateUnit(SelectedUnit);

        NotificationService.Toast(NotificationType.Success, $"وضعیت واحد {SelectedUnit.Title} در ساختمان {BuildingName} با موفقیت به وضعیت {(SelectedUnit.IsActive ? "فعال" : "غیرفعال")} تغییر یافت.");
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var unit = (UnitReadDto)obj.Item;

        if (unit.IsActive is false)
            obj.Class = "contract-ending-alert";
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }
}
