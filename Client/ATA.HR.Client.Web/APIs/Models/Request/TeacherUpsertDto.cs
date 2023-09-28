﻿using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class TeacherUpsertDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "نام مدرس را وارد کنید")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "نام خانوادگی مدرس را وارد کنید")]
    public string LastName { get; set; }

    public string FatherName { get; set; }
    public string IdNo { get; set; }

    [Required(ErrorMessage = "کد ملی مدرس را وارد کنید")]
    public string NationalCode { get; set; }

    public DateTime? BirthDate { get; set; }
    public string IssuePlace { get; set; }
    public bool IsMan { get; set; }
    public bool IsMarried { get; set; }
    public string ReligionNationality { get; set; }
    public string Education { get; set; }
    public string Field { get; set; }
    public string EducationPlace { get; set; }
    public string Expertise { get; set; }
    public string physicalCondition { get; set; }
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
}