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
public partial class RoomPage : IDisposable
{
    // Consts
    private const int PageSize = 40;

    private RoomReadDto SelectedRoom;

    // Props
    public int TotalRoomCounts { get; set; }
    public bool IsChangeRoomStatusWindowVisible { get; set; } = false;
    public bool SelectedRoomStatus { get; set; }
    public bool IsLoading { get; set; } = true;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public OperationType PageOperationType { get; set; } = OperationType.Filter;

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public RoomFilterArgs RoomDataFilter { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    // Index
    private TelerikGrid<RoomReadDto> RoomGridRef { get; set; }
    private List<RoomReadDto> RoomList { get; set; } = new();
    public RoomDto Room { get; set; } = new();
    public string City { get; set; }
    public string CityName { get; set; }
    public string BuildingName { get; set; }
    public string UnitName { get; set; }

    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public int BuildingId { get; set; }
    [Parameter] public int UnitId { get; set; }

    // Life Cycles
    protected async override Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            RoomDataFilter.BuildingId = Room.BuildingId = BuildingId;
            RoomDataFilter.UnitId = Room.UnitId = UnitId;

            var buildingInfo = await HttpClient.Building().GetById(BuildingId, cancellationToken: cancellationToken);

            if (buildingInfo != null)
            {
                var city = (ReferenceCity)buildingInfo.CityId;

                City = city.ToString();
                CityName = city.ToDisplayName();
                BuildingName = buildingInfo.Title;
            }

            var unitInfo = await HttpClient.Unit().GetById(UnitId, cancellationToken: cancellationToken);
            UnitName = unitInfo != null ? unitInfo.Title : string.Empty;

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

        return RoomGridRef.SetState(RoomGridRef.GetState());
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;
    private void ChangeToAddRoomMode() => PageOperationType = OperationType.Add;
    public Task ApplyFilters => RebindGrid(true);

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                RoomGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            if (UnitId > 0)
                args.Data = RoomList = await HttpClient.Room().GetUnitRooms(RoomDataFilter, context);
            else
                args.Data = RoomList = await HttpClient.Room().GetBuildingRooms(RoomDataFilter, context);

            args.Total = TotalRoomCounts = (int)(context.TotalCount ?? 0);
        }
        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public async Task OnRoomSubmit()
    {
        try
        {
            await HttpClient.Room().AddRoom(Room);

            var msg = UnitId > 0 ? $"اتاق {Room.Title} در ساختمان {BuildingName} - واحد {UnitName} با موفقیت ذخیره گردید."
                : $"اتاق {Room.Title} در ساختمان {BuildingName} با موفقیت ذخیره گردید.";

            NotificationService.Toast(NotificationType.Success, msg);

            ChangeToFilterMode();
            Room = new();

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

        if (RoomDataFilter.SearchTerm != searchTerm)
            RoomDataFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(RoomDataFilter.SearchTerm);
    }

    public void OpenRoomPage(int RoomId)
    {
        if (RoomId == 0) return;

        NavigationManager.NavigateTo(PageUrls.RoomPage(BuildingId, RoomId));
    }

    public void ChangeRoomStatus(RoomReadDto Room)
    {
        if (Room == null) return;

        SelectedRoom = Room;
        IsChangeRoomStatusWindowVisible = true;

        SelectedRoomStatus = RoomList.FirstOrDefault(u => u.Id == Room.Id).IsActive;
    }

    public async Task ConfirmChangeRoomStatus()
    {
        SelectedRoom.IsActive = !SelectedRoomStatus;

        await HttpClient.Room().UpdateRoom(SelectedRoom);

        var msg = UnitId > 0 ? $"وضعیت اتاق {SelectedRoom.Title} در ساختمان {BuildingName} - واحد {UnitName} با موفقیت به وضعیت {(SelectedRoom.IsActive ? "فعال" : "غیرفعال")} تغییر یافت."
                : $"وضعیت اتاق {SelectedRoom.Title} در ساختمان {BuildingName} با موفقیت به وضعیت {(SelectedRoom.IsActive ? "فعال" : "غیرفعال")} تغییر یافت.";

        NotificationService.Toast(NotificationType.Success, msg);
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var room = (RoomReadDto)obj.Item;

        if (room.IsActive is false)
            obj.Class = "contract-ending-alert";
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }
}
