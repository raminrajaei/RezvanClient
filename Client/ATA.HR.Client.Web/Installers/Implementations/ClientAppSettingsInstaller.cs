using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Implementations;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class ClientAppSettingsInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        // Directly with injecting ClientAppSettings class
        services.AddSingleton(serviceProvider => clientAppSettings);

        services.AddTransient<IAppDataService, AppDataService>();
    }
}