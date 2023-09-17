using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class SignatureInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        services.AddBootstrapBlazor();

        //services.AddRequestLocalization<IOptionsMonitor<BootstrapBlazorOptions>>((localizerOption, blazorOption) =>
        //{
        //    blazorOption.OnChange(op => Invoke(op));
        //    Invoke(blazorOption.CurrentValue);

        //    void Invoke(BootstrapBlazorOptions option)
        //    {
        //        var supportedCultures = option.GetSupportedCultures();
        //        localizerOption.SupportedCultures = supportedCultures;
        //        localizerOption.SupportedUICultures = supportedCultures;
        //    }
        //}
    }
}