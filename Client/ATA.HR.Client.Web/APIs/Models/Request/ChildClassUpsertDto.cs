namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ChildClassUpsertDto
{
    public long Id { get; set; }

    public long ClassRoomId { get; set; }
    public string ClassRoomIdSelectedValue { get; set; }

    public long ChildId { get; set; }
    public string ChildIdSelectedValue { get; set; }

    public string? FromDateJalali { get; set; }
    public DateTime From { get; set; }

    public string? ToDateJalali { get; set; }
    public DateTime To { get; set; }

    public long TeacherId { get; set; }
    public string TeacherIdSelectedValue { get; set; }

    public int Tuition { get; set; }
}