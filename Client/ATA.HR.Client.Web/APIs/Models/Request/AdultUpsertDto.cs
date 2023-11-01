using System.ComponentModel.DataAnnotations;
using ATA.HR.Client.Web.APIs.Enums;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class AdultUpsertDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام را وارد نمایید")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
    public string LastName { get; set; }
    public string FatherName { get; set; }

    public string IdNo { get; set; }

    [Required(ErrorMessage = "تاریخ تولد را مشخص نمایید")] 
    public string? BirthDateJalali { get; set; }
    public DateTime BirthDate { get; set; }

    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }

    [Required(ErrorMessage = "کد ملی را وارد نمایید")]
    public string NationalCode { get; set; }

    public string Field { get; set; }
    public string EducationLevel { get; set; }
    public string Job { get; set; }

    public MaritalStatusEnum? MaritalStatus { get; set; }
    public string? MaritalStatusSelectedValue { get; set; }

    public string PhoneNumber { get; set; }
    public string HomeAddress { get; set; }
    public string HomePhone { get; set; }
    public string HomePostalCode { get; set; }
    public string WorkAddress { get; set; }
    public string WorkPhone { get; set; }
    public string WorkPostalCode { get; set; }
    public string PhotoPath { get; set; }
    public string FamiliarityInstitution { get; set; }

    // Nav
    public AdultMoreInfoUpsertDto AdultMoreInfo { get; set; } = new();
}