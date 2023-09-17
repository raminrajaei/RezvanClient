using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingDto
{
    public int Id { get; set; }
    public DateTime? CheckinDate => string.IsNullOrWhiteSpace(CheckinDateJalali) ? null : CheckinDateJalali.ToGregorianDateTime(false);
    public string? CheckinDateJalali { get; set; } = string.Empty;
    public int Duration { get; set; }
    public string? Description { get; set; }
    public int BedId { get; set; }
    public int RoomId { get; set; }
    public int UnitId { get; set; }
    public string? BedName { get; set; }
    public string? GuestName { get; set; }
    public string? RoomName { get; set; }
    public string? UnitName { get; set; }
    public string? BuildingName { get; set; }
    public int BuildingId { get; set; }
    public int GuestCounts { get; set; }
    public int BedCounts { get; set; }
    public int RoomBookingStatus { get; set; }
    public bool AreAllBedsBooked { get; set; }
    public string RoomBookingStatusName => ((RoomBookingStatus)RoomBookingStatus).ToDisplayName(true)!;
}
