using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.PaySlip;

[ComplexType]
public class PaySlipReadDto
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? EmploymentNumber { get; set; }

    public string? DLTitle { get; set; }

    public string? YearMonthTitle { get; set; }

    public int? Year { get; set; }

    public string? CompensationFactor { get; set; }

    public decimal Value { get; set; }

    public string? Category { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? WorkLocation { get; set; }

    public int Month { get; set; }

    public int PayOrConst { get; set; }

    public string? Mobile { get; set; }
}