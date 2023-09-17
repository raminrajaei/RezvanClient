namespace ATA.HR.Shared.Localization.Contract
{
    public interface IStringsProvider
    {
        string? Message(string messageStringsKey);

        string Exception(string exceptionStringsKey);
    }
}