using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingRoomFilterArgs
{
    public int RoomId { get; set; }
    public int TakeAmount { get; set; }
}
