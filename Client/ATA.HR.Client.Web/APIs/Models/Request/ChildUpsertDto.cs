using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ChildUpsertDto
{
    public long Id { get; set; } = 0;

    [Required(ErrorMessage = "نام کودک را وارد کنید")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "نام خانوادگی کودک را وارد کنید")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "تاریخ تولد کودک را مشخص نمایید")]
    public string? BirthDateJalali { get; set; }
    public DateTime BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }
    public string SerialNo { get; set; }
    public string SerialCode { get; set; }

    [Required(ErrorMessage = "شماره ملی کودک را وارد کنید")]
    public string IdNo { get; set; }

    #region Father Info
    [Required(ErrorMessage = "نام پدر کودک را وارد کنید")]
    public string FatherName { get; set; }

    public DateTime? FatherBirthDate { get; set; }
    public string? FatherBirthDateJalali { get; set; }
    public string FatherBirthPlace { get; set; }
    public string FatherIssuePlace { get; set; }
    public string FatherSerialNo { get; set; }
    public string FatherIdNo { get; set; }

    [Required(ErrorMessage = "کد ملی پدر کودک را وارد کنید")]
    public string FatherNationalCode { get; set; }
    public string FatherEducation { get; set; }
    public string FatherField { get; set; }
    public string FatherJob { get; set; }
    public string FatherMobileNo { get; set; }
    public string FatherWorkAddress { get; set; }
    #endregion

    #region Mother Info
    [Required(ErrorMessage = "نام مادر کودک را وارد کنید")]
    public string MotherName { get; set; }
    public DateTime? MotherBirthDate { get; set; }
    public string? MotherBirthDateJalali { get; set; }
    public string MotherBirthPlace { get; set; }
    public string MotherIssuePlace { get; set; }
    public string MotherSerialNo { get; set; }
    public string MotherIdNo { get; set; }

    [Required(ErrorMessage = "کد ملی مادر کودک را وارد کنید")]
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
    public string PhotoPath { get; set; }


    public ChildMoreInfoUpsertDto ChildMoreInfo { get; set; } = new();
    public List<ChildDeliverAddDto> ChildDelivers { get; set; } = new();

}