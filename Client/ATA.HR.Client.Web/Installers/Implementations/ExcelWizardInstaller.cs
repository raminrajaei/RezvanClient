using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using ExcelWizard;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class ExcelWizardInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        services.AddExcelWizardServices(true);
    }
}