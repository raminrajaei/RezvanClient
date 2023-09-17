using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using Bit.Http.Contracts;
using Bit.Http.Implementations;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class BitInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            services.AddScoped<ISecurityService, DefaultSecurityService>();
            services.AddTransient<ITokenProvider, DefaultTokenProvider>();
        }
    }
}