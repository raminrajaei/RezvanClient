using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Implementations;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using Blazored.Toast;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class NotificationInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        //https://github.com/Blazored/Toast
        services.AddBlazoredToast();

        services.AddTransient<INotificationService, NotificationService>();
    }
}