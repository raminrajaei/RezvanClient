using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingRoomDto
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public string? RoomName {get;set;}
    public int UnitId { get; set; }
    public string? UnitName {get;set;}
    public string? BuildingName {get;set;}
    public int BuildingId { get; set; }
    public int RoomBookingStatus { get; set; }
    public int GuestCounts { get; set; }
    public int BedCounts { get; set; }

    public List<BookingDto> Bookings { get; set; } = new();
}
