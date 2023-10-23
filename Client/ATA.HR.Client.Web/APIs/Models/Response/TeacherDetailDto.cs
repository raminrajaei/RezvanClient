using ATA.HR.Client.Web.APIs.Enums;
using ATABit.Helper.Extensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

public class TeacherDetailDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FatherName { get; set; }
    public string IdNo { get; set; }
    public string NationalCode { get; set; }
    public string BankCardNo { get; set; }
    public DateTime? BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }
    public GenderEnum? Gender { get; set; }
    public string GenderDisplay => Gender.HasValue
        ? Gender.Value.ToDisplayName()
        : "";

    public MaritalStatusEnum? MaritalStatus { get; set; }
    public string MaritalStatusDisplay => MaritalStatus.HasValue
        ? MaritalStatus.Value.ToDisplayName()
        : "";

    public string ReligionNationality { get; set; }
    public string Education { get; set; }
    public string Field { get; set; }
    public string EducationPlace { get; set; }
    public string Expertise { get; set; }
    public string PhysicalCondition { get; set; }
    public string HomeAddress { get; set; }
    public string HomePhone { get; set; }
    public string WorkAddress { get; set; }
    public string WorkPhone { get; set; }
    public string PhoneNumber { get; set; }
    public string SocialNetworkNumber { get; set; }
    public bool IsMomtahen { get; set; }
    public bool IsModares { get; set; }
    public bool IsMoalem { get; set; }
    public bool IsMorabi { get; set; }
    public string PhotoPath { get; set; }

    public string Activities { get; set; }
}