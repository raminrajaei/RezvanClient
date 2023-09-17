using ATA.HR.Client.Web.APIs.Auth;
using System.Net.Http;

namespace ATA.HR.Client.Web.APIs;

public interface ICoreAPIs : IInsuranceAPIs, IAuthAPIs
{
    // This will automatically be populated by Refit if the property exists
    HttpClient Client { get; }
}