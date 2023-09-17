using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BookingFormDto
{
    public bool AreAllBedsBooked { get; set; }
    public List<BookingDto>? Bookings { get; set; } = new List<BookingDto>();
}