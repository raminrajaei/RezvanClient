using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class MyContractsFilterArgs
{
    public bool Active { get; set; } = true;

    public bool Pending { get; set; }

    public bool All { get; set; }
}