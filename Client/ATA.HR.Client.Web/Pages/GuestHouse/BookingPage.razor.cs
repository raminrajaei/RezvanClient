using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Implementations;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using Bit.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Pages.GuestHouse;

[Authorize]
public partial class BookingPage
{
    // Props
    private bool IsLoading { get; set; } = false;
    private bool IsVacateRoomWindowVisible { get; set; } = false;

    private BookingDto CurrentSelectedRoom = new();

    public BookingFilterArgs BookRoomFilterArgs { get; set; } = new();
    public List<BookingReadDto> RoomBookingInfos { get; set; } = new();

    public ReferenceCity RefCity { get; set; }
    public string CityDisplay { get; set; }
    public int CurrentRoomOccupiedBedCounts { get; set; }

    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }

    [Parameter] public string City { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            if (Enum.TryParse(typeof(ReferenceCity), City, out var referenceCity))
            {
                RefCity = (ReferenceCity)Enum.Parse(typeof(ReferenceCity), City);

                BookRoomFilterArgs.CityId = (int)RefCity;
                CityDisplay = RefCity.ToDisplayName(true);
            }
            else
                throw new DomainLogicException("Invalid city query string");

            RoomBookingInfos = (await HttpClient.Booking().GetAllRoomBookingInfo(BookRoomFilterArgs))
                .Bookings.GroupBy(g => new { g.BuildingName, g.UnitName })
                .Select(g => new BookingReadDto()
                {
                    BuildingName = g.Key.BuildingName,
                    UnitName = g.Key.UnitName,
                    Bookings = g.ToList()

                }).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    public async void ConfirmVacateRoom()
    {
        await HttpClient.Booking().VacantRoom(new VacantInfoDto() { Id = CurrentSelectedRoom.Id });

        IsVacateRoomWindowVisible = false;

        NotificationService.Toast(NotificationType.Success, $"کلیه ی {CurrentSelectedRoom.BedCounts} تخت اتاق {CurrentSelectedRoom.RoomName} تخلیه شد.");
    }

    public void VacateRoom(BookingDto room)
    {
        CurrentRoomOccupiedBedCounts = room.GuestCounts;
        IsVacateRoomWindowVisible = true;
    }

    public void NavigateToBookingForm(int roomId)
    {
        NavigationManager.NavigateTo(PageUrls.BookingFormPage(City, roomId));
    }
}
