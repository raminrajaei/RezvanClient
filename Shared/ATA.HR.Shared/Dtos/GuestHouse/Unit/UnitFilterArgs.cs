using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UnitFilterArgs
{
    public int BuildingId { get; set; }

    public string? SearchTerm { get; set; }
}
