using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models;
using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class CacheInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            // Singleton cash using by directly injecting AppData into a class
            var appCache = new AppData();

            services.AddSingleton(sp => appCache);
        }
    }
}