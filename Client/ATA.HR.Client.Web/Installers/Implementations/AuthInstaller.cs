using ATA.HR.Client.Web.Implementations;
using ATA.HR.Client.Web.Installers.Contract;
using ATA.HR.Client.Web.Models.AppSettings;
using ATA.HR.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace ATA.HR.Client.Web.Installers.Implementations;

public class AuthInstaller : IClientInstaller
{
    public void InstallServices(IServiceCollection services, ClientAppSettings clientAppSettings)
    {
        services.AddOptions();

        services.AddAuthorizationCore(config =>
            {
                var claims = Claims.GetAllAppClaims().ToList();

                foreach (var claim in claims)
                {
                    config.AddPolicy(claim,
                        new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .RequireClaim(claim)
                            .Build());
                }
            }
        );

        services.AddScoped<AuthenticationStateProvider, AppAuthenticationStateProvider>();

        services.AddTransient(serviceProvider => (AppAuthenticationStateProvider)serviceProvider.GetRequiredService<AuthenticationStateProvider>());
    }
}