namespace ATA.HR.Client.Web.APIs.Models.Response;

public class ChildDetailDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }
    public string SerialNo { get; set; }
    public string IdNo { get; set; }
    public string PhotoPath { get; set; }

    #region Father Info
    public string FatherName { get; set; }
    public DateTime? FatherBirthDate { get; set; }
    public string FatherBirthPlace { get; }
    public string FatherIssuePlace { get; }
    public string FatherSerialNo { get; set; }
    public string FatherIdNo { get; set; }
    public string FatherNationalCode { get; set; }
    public string FatherEducation { get; set; }
    public string FatherField { get; set; }
    public string FatherJob { get; set; }
    public string FatherMobileNo { get; set; }
    public string FatherWorkAddress { get; set; }
    #endregion

    #region Mother Info
    public string MotherName { get; set; }
    public DateTime? MotherBirthDate { get; set; }
    public string MotherBirthPlace { get; }
    public string MotherIssuePlace { get; }
    public string MotherSerialNo { get; set; }
    public string MotherIdNo { get; set; }
    public string MotherNationalCode { get; set; }
    public string MotherEducation { get; set; }
    public string MotherField { get; set; }
    public string MotherJob { get; set; }
    public string MotherMobileNo { get; set; }
    public string MotherWorkAddress { get; set; }
    #endregion

    public byte? FamilyChildrenCount { get; set; }
    public byte? NumberOfChildren { get; set; }
    public string HomeAddress { get; set; }
    public string HomeTel { get; set; }
    public string HomePostalCode { get; set; }

    public ChildMoreInfoDto ChildMoreInfo { get; set; }
    public List<ChildDeliverDto> ChildDelivers { get; set; }
}