using ATA.HR.Server.Model.AppSettingsOptions;
using Bit.Core.Contracts;

namespace ATA.HR.Server.Api.Installers.Contract
{
    public interface IServerInstaller
    {
        void InstallServices(IServiceCollection services,
            IDependencyManager dependencyManager,
            ServerAppSettings serverAppSettings);
    }
}