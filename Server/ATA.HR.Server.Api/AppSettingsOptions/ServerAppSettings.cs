namespace ATA.HR.Server.Model.AppSettingsOptions;

public record ServerAppSettings
{
    public AuthOptions? AuthOptions { get; set; }

    public bool IsDevelopment { get; set; }
}