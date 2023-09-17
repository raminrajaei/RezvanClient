using ATA.HR.Server.Api.Installers.Contract;
using ATA.HR.Server.Model.AppSettingsOptions;
using Bit.Core.Contracts;

namespace ATA.HR.Server.Api.Installers.Implementations
{
    public class AppSettingsJsonInstaller : IServerInstaller
    {
        public void InstallServices(IServiceCollection services, IDependencyManager dependencyManager,
            ServerAppSettings serverAppSettings)
        {
            // Register (Server)AppSettings as Singleton to easy use
            dependencyManager.RegisterInstance(serverAppSettings);
        }
    }
}