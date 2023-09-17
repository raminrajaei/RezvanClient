using Bit.Utils.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.AppVersioning;

[ComplexType]
public class AppVersioningDto
{
    public string? Version { get; set; }
    public int VersionNo => Version!.Replace(".", "").ToInt();

    public string? Date { get; set; }

    public List<VersionChange> Changes { get; set; } = new();
}

[ComplexType]
public class VersionChange
{
    [Required(ErrorMessage = "Version Change Title Is Required")]
    public string? Title { get; set; }

    public List<string> Description { get; set; } = new();

    public List<string> ClaimsCanView { get; set; } = new();

    public List<string> UnitsCanView { get; set; } = new();
}