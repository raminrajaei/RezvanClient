using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UserToSendContractDto : UserDto
{
    public List<ContractReadDto> Contracts { get; set; } = new();

    // If User active contracts is zero, he/she is eligible for sending new contract: UPDATE: Due to lack of CEO cooperation it is OK now to have multiple active contracts 
    public bool HasAnyPendingContract => Contracts.Any(c => c.FlowStatus == (int)FlowStatus.Pending);

    public int PendingContractsCount => Contracts.Count(c => c.FlowStatus == (int)FlowStatus.Pending);

    public List<KeyValuePair<int, string>> ContractDateDisplay => Contracts.Select(c => new KeyValuePair<int, string>(c.Id, $"از {c.ContractDetailsExecutionDateJalaliDisplay} تا {c.ContractDetailsValidityDateJalaliDisplay}")).ToList();

    // User Contracts Count
    //public int ContractsCount { get; set; } //Flattening
}