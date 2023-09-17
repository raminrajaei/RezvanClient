namespace ATA.HR.Shared.Dtos.Document;

public class UploadedFileResult
{
    public Guid Identifier { get; set; }

    public string? FileUrl { get; set; }
}