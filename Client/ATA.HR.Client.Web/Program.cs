using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Implementations;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models;
using ATA.HR.Client.Web.Models.AppSettings;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Refit;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace ATA.HR.Client.Web;

public class Program
{
    public static Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var clientAppSettings = builder.Configuration.GetSection(nameof(ClientAppSettings)).Get<ClientAppSettings>();
        clientAppSettings.UrlSettings!.AppURL = builder.HostEnvironment.BaseAddress;

        // Host client
        builder.Services.AddHttpClient<HostClient>(httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<AppHttpExceptionHandler>();

        // Insurance client
        builder.Services.AddRefitClient<ICoreAPIs>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(clientAppSettings.UrlSettings.ATACoreAPIsURL!))
            .AddHttpMessageHandler<CoreAPIsHttpHandler>();
        
        // Rezvan client
        builder.Services.AddRefitClient<IRezvanAPIs>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(clientAppSettings.UrlSettings.RezvanAPIsURL!))
            .AddHttpMessageHandler<RezvanAPIsHttpHandler>();

        // Configure Dependencies with Service Installers
        var installers = new[] { Assembly.GetExecutingAssembly() }.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IClientInstaller).IsAssignableFrom(c))
            .Select(Activator.CreateInstance).Cast<IClientInstaller>().ToList();
        installers.ForEach(i => i.InstallServices(builder.Services, clientAppSettings));

        builder.UseLoadingBar();

        return builder
            .Build()
            .RunAsync();
    }
}