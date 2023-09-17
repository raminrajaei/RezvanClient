using ATA.Broker.SSOSecurity;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class BrokersInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        services.AddATASSOClient();
    }
}