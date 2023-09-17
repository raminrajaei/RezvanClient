using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingReadDto
{
    public string? BuildingName { get; set; }
    public string? UnitName { get; set; }

    public List<BookingDto>? Bookings { get; set; }
}
