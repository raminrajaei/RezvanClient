using ATABit.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UserWithSignatureDto : UserDto
{
    public bool HasUserSignature { get; set; }
}