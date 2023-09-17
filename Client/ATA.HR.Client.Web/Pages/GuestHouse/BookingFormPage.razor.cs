using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Pages.GuestHouse;

[Authorize]
public partial class BookingFormPage
{
    // Props
    private bool IsLoading { get; set; } = false;

    public RoomReadDto Room { get; set; } = new();
    public BookingFormDto BookingModel { get; set; } = new();

    public string BuildingName { get; set; }
    public string UnitName { get; set; }
    public string RoomName { get; set; }

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public INotificationService NotificationService { get; set; }

    [Parameter] public int RoomId { get; set; }
    [Parameter] public string City { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            Room = await HttpClient.Room().GetById(RoomId);

            BuildingName = Room.BuildingName.ToPersianNumbers();
            UnitName = Room.UnitName.ToPersianNumbers();
            RoomName = Room.Title.ToPersianNumbers();

            await GetRoomBookingInfo();
        }
        catch
        {
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    public async Task GetRoomBookingInfo()
    {
        BookingModel.Bookings = await HttpClient.Booking()
            .GetRoomBookingInfoByRoomIdAndDate(new BookingRoomFilterArgs()
            {
                TakeAmount = Room.OccupiedBedCounts,
                RoomId = RoomId,
            });

        BookingModel.AreAllBedsBooked = Room.AreAllBedsBooked;

        if (BookingModel.Bookings.Count < Room.BedCounts)
        {
            foreach (var item in Room.Beds)
            {
                if (BookingModel.Bookings.Any(b => b.BedId == item.Id) is false)
                {
                    BookingModel.Bookings.Add(new BookingDto()
                    {
                        UnitId = Room.UnitId,
                        RoomId = RoomId,
                        BedId = item.Id,
                        BedName = item.Title,
                    });
                }
            }
        }
    }

    public async Task BookBeds()
    {
        if (BookingModel.Bookings.Any(b => string.IsNullOrWhiteSpace(b.GuestName) is false
        && b.Duration > 0 && string.IsNullOrWhiteSpace(b.CheckinDateJalali) is false) is false)
        {
            NotificationService.Toast(NotificationType.Error, $"اطلاعات رزرو حداقل یک تخت باید وارد شود.");
            return;
        }

        await HttpClient.Booking().AddOrUpdate(BookingModel);

        NavigateBackToBookingPage();
    }

    public void NavigateBackToBookingPage()
    {
        NavigationManager.NavigateTo($"/guest-houses/booking/{City}");
    }
}