using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingFilterArgs
{
    public int RoomBookingType { get; set; }

    public int CityId { get; set; }
}
