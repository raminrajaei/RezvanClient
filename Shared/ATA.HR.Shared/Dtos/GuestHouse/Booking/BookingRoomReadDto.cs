using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingRoomReadDto
{
    public int Id { get; set; }
    public string? BuildingName { get; set; }
    public string? UnitName { get; set; }
    public string? RoomName { get; set; }
    public int GuestCounts { get; set; }
    public int BedCounts { get; set; }
    public DateTime CheckinDate { get; set; }
    public int Duration { get; set; }
    public string? Description { get; set; }
    public int RoomBookingStatus { get; set; }
}
