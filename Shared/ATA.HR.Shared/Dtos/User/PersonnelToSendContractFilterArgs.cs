using ATA.HR.Shared.Enums.Contract;
using ATA.HR.Shared.Enums.Workflow;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class PersonnelToSendContractFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? Unit { get; set; }

    public string? BoxStatusSelectedValue { get; set; } = UserBoxStatus.NotImportant.ToString("d");

    public string? HokmStatusSelectedValue { get; set; } = HokmTypeStatusToSendContract.NotImportant.ToString("d");

    public bool? OnlyPersonnelWithNoContractsAtAll { get; set; }

    public bool? OnlyPersonnelWithContractsClosingToEnd { get; set; }
}