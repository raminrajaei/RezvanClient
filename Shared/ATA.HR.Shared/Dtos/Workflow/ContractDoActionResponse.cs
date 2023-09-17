using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Workflow;

[ComplexType]
public record ContractDoActionResponse(bool OpenTheSMSBoxToEnterCode, int SmsCodeIsValidForSeconds, bool IsUsingNationalCodeInsteadOfSmsCode, string? UserMobile);