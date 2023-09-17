namespace ATA.HR.Client.Web.APIs.Insurance.Models.Responses;

public class CostAttachmentDto
{
    public int Id { get; set; }

    public int CostId { get; set; }

    public string? Title { get; set; }

    public string? FileType { get; set; }

    public string? FileExtension { get; set; }

    public string? Url { get; set; }
}