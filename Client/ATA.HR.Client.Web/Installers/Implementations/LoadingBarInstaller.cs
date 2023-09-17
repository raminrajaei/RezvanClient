using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class LoadingBarInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            services.AddLoadingBar();
        }
    }
}