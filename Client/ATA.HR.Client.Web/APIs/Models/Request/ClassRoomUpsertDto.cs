using System.ComponentModel.DataAnnotations;
using ATA.HR.Client.Web.APIs.Enums;

namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ClassRoomUpsertDto
{
    public long Id { get; set; }

    [Required(ErrorMessage = "عنوان کلاس را وارد نمایید")]
    public string Title { get; set; }

    public long? ParentId { get; set; }

    [Required(ErrorMessage = "نوع کلاس را وارد نمایید")]
    public int ClassType { get; set; }
}