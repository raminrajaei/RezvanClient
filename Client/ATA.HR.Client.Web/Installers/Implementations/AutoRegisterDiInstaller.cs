using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace ATA.HR.Client.Web.Installers.Implementations
{
    public class AutoRegisterDiInstaller : IClientInstaller
    {
        public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
        {
            var webAssembly = Assembly.GetExecutingAssembly();
            var clientWebServiceAssembly = typeof(ClientAppSettings).Assembly;

            var assembliesToScan = new[] { webAssembly, clientWebServiceAssembly };

            #region Generic Type Dependencies
            //services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepositoryBase<,>));
            #endregion


            #region Register DIs By Name
            services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
                .Where(c => c.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
            #endregion 

        }
    }
}