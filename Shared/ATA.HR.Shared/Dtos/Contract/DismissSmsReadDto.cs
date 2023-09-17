using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Contract;

[ComplexType]
public class DismissSmsReadDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    public string CreatedAtJalali => CreatedAt.ToJalaliString(false);

    public int UserId { get; set; }

    public string? SmsContent { get; set; }

    public string? UserFullName { get; set; } //Flattening

    public string? UserPersonnelCode { get; set; } //Flattening
}