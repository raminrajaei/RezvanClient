using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Enums.Contract;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Threading;

namespace ATA.HR.Client.Web.Components.Contracts;

public partial class ContractV1
{
    [Parameter] public int ContractId { get; set; }

    [Parameter] public bool IsPreview { get; set; }

    [Parameter] public bool HasCurrentUserActiveSignature { get; set; }

    [Parameter] public bool IsCurrentUserActor { get; set; }

    public string ContractOpacityStyle => HasCurrentUserActiveSignature is false && IsCurrentUserActor ? "opacity: 0.3" : "";

    [Parameter] public int? ToDoPersonnelCode { get; set; } //The personnel which should confirm the contract. Is used for Signature show

    public ContractReadDto? UserContract { get; set; }

    public bool IsHourlyContract { get; set; }

    public string MaxHours { get; set; } = "120";

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        UserContract = await HttpClient.Contract().GetContractById(ContractId, cancellationToken: cancellationToken);

        if (UserContract?.UserId == 27)
            MaxHours = "150";

        IsHourlyContract = UserContract?.ContractDetailsEmploymentTypeCode == (long)EmploymentType.Hourly;

        await base.OnInitializedAsync(cancellationToken);
    }
}