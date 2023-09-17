using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Implementations;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models;
using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class HttpClientFactoryInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            #region Host client

            services.AddScoped(serviceProvider => serviceProvider.GetService<HostClient>()!.Client);

            services.AddScoped<AppHttpExceptionHandler>();

            #endregion

            #region APICores client

            services.AddScoped<CoreAPIsHttpHandler>();

            #endregion
            
            #region Rezvan client

            services.AddScoped<RezvanAPIsHttpHandler>();

            #endregion
        }
    }
}