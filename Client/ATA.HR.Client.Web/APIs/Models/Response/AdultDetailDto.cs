using ATA.HR.Client.Web.APIs.Enums;
using ATABit.Helper.Extensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

public class AdultDetailDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string IdNo { get; set; }
    public DateTime BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }
    public string NationalCode { get; set; }
    public string Field { get; set; }
    public string EducationLevel { get; set; }
    public string Job { get; set; }

    public MaritalStatusEnum? MaritalStatus { get; set; }
    public string MaritalStatusDisplay => MaritalStatus.HasValue
    ? MaritalStatus.Value.ToDisplayName() : string.Empty;

    public string PhoneNumber { get; set; }
    public string HomeAddress { get; set; }
    public string HomePhone { get; set; }
    public string HomePostalCode { get; set; }
    public string WorkAddress { get; set; }
    public string WorkPhone { get; set; }
    public string WorkPostalCode { get; set; }
    public string PhotoPath { get; set; }
    public string FamiliarityInstitution { get; set; }

    public AdultMoreInfoDto AdultMoreInfo { get; set; } = new();
}