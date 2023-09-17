using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using Blazored.Modal;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class ModalInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            // https://github.com/Blazored/Modal
            services.AddBlazoredModal();
        }
    }
}