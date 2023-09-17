using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Models;
using System.Net.Http;

namespace ATA.HR.Client.Web.Implementations
{
    public class AppDataService : IAppDataService
    {
        private AppData _appData;
        private HttpClient _httpClient;

        public AppDataService(AppData appData, HttpClient httpClient)
        {
            _appData = appData;
            _httpClient = httpClient;
        }

        //public async Task<List<LocationReadDto>> GetAllLocations()
        //{
        //    List<LocationReadDto> locations = new();

        //    if (_appData.Locations.Any() is false)
        //        locations = await _httpClient.Location().GetAllLocations();
        //    else
        //        locations = _appData.Locations;

        //    return locations;
        //}
    }
}
