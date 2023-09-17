using ATA.HR.Shared.Enums.AppGeneric;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.AppGeneric;

[ComplexType]
public class AppSMSReadDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedAtJalali => CreatedAt.ToJalaliString(false);

    public int? UserIdSender { get; set; }

    public int? UserIdReceiver { get; set; }
    public string? UserReceiverPersonnelCode { get; set; } //Flattening
    public string? UserReceiverFullName { get; set; } //Flattening

    [Required]
    public string? MobileReceiver { get; set; }

    public bool ByBackgroundJob { get; set; }
    public string ByBackgroundJobDisplay => ByBackgroundJob ? "بله" : "خیر";

    public Guid? BatchIdentifier { get; set; }

    public string? RefIdentifier { get; set; }

    public int AppSMSType { get; set; }
    public string? AppSMSTypeDisplay => ((AppSMSType)AppSMSType).ToDisplayName();

    public string? MessageContent { get; set; }

    public string? SmsProviderResponse { get; set; }
}