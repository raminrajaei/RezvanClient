using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using Blazored.LocalStorage;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class LocalStorageInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            // https://github.com/Blazored/LocalStorage
            services.AddBlazoredLocalStorage();
        }
    }
}