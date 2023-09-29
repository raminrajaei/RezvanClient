using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ChildClassUpsertDto
{
    public long Id { get; set; }

    public long ClassRoomId { get; set; }

    [Required(ErrorMessage = "کلاس را انتخاب نمایید")]
    public string? ClassRoomIdSelectedValue { get; set; }

    public long ChildId { get; set; }

    [Required(ErrorMessage = "کودک را انتخاب نمایید")]
    public string ChildIdSelectedValue { get; set; }

    [Required(ErrorMessage = "از تاریخ را وارد نمایید")]
    public string? FromDateJalali { get; set; }
    public DateTime From { get; set; }

    [Required(ErrorMessage = "تا تاریخ را وارد نمایید")]
    public string? ToDateJalali { get; set; }
    public DateTime To { get; set; }

    public long TeacherId { get; set; }

    [Required(ErrorMessage = "مربی را انتخاب نمایید")]
    public string? TeacherIdSelectedValue { get; set; }

    [Required(ErrorMessage = "شهریه را وارد نمایید")]
    public int Tuition { get; set; }
}