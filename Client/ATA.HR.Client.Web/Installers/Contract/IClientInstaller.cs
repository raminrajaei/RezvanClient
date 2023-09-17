using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Contract
{
    public interface IClientInstaller
    {
        void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings);
    }
}