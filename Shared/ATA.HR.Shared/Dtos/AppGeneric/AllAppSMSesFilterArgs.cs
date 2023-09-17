using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class AllAppSMSesFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? AppSMSTypeSelectedValue { get; set; }
}