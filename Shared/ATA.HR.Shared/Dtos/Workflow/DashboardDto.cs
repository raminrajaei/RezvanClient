using ATA.HR.Shared.Dtos.WorkHours;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public class DashboardDto
{
    public int RefId { get; set; } //ContractId, WorkHoursId, etc

    public int FlowType { get; set; }

    public int? UserId { get; set; }

    public int? PersonnelCode { get; set; }

    public string? LastName { get; set; } //To sort 

    public string? FullName { get; set; }

    public string? Unit { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedAtJalali => CreatedAt.ToJalaliString();

    public int WorkStatusCode { get; set; }

    public string? WorkStatusName { get; set; }

    public string? LastStateName { get; set; }

    public bool IsFromWorkList { get; set; }

    public string? WorkType { get; set; }

    // Contract
    public int FlowGeneralStatus { get; set; }
    public string? FlowGeneralStatusDisplay => ((FlowStatus)FlowGeneralStatus).ToDisplayName();
    public string? IssuanceDateJalali { get; set; }
    public string? ValidityDateJalali { get; set; }
    public string ContractDateJalaliPersian => $"{IssuanceDateJalali.ToPersianNumbers()} الی {ValidityDateJalali.ToPersianNumbers()}";

    // WorkHours
    public WorkHourReadDto? WorkHour { get; set; }
}