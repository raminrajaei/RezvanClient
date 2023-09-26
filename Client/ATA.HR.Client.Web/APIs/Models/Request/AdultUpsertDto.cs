﻿using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class AdultUpsertDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام را وارد نمایید")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "شماره شناسنامه را وارد نمایید")]
    public string IdNo { get; set; }
    public DateTime BirthDate { get; set; }
    public string BirthPlace { get; set; }
    public string IssuePlace { get; set; }

    [Required(ErrorMessage = "کد ملی را وارد نمایید")]
    public string NationalCode { get; set; }

    public string Field { get; set; }
    public string EducationLevel { get; set; }
    public string Job { get; set; }
    public bool IsSingle { get; set; }
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
    public AdultMoreInfoUpsertDto AdultMoreInfo { get; set; }
}