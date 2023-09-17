namespace ATA.HR.Server.Model.AppSettingsOptions;

public record AuthOptions
{
    public string? MasterPassword { get; set; }

    public bool? EnableDirectLogin { get; set; }

    public bool? IsAppPublic { get; set; }
}