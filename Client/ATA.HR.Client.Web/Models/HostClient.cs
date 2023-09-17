using System.Net.Http;

namespace ATA.HR.Client.Web.Models
{
    public class HostClient
    {
        public HttpClient Client { get; }

        public HostClient(HttpClient client)
        {
            Client = client;
        }
    }
}