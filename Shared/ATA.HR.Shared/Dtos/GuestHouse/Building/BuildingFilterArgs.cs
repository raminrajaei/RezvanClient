using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BuildingFilterArgs
{
    public int CityId { get; set; }

    public string? SearchTerm { get; set; }
}
