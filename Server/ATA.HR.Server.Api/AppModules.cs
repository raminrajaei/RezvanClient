using ATA.HR.Server.Api.Installers.Contract;
using ATA.HR.Server.Model.AppSettingsOptions;
using Bit.Core.Contracts;
using Bit.Owin.Contracts;
using Bit.Owin.Implementations;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.Owin.Cors;
using Owin;
using Swashbuckle.Application;
using System.IO.Compression;
using System.Reflection;

namespace ATA.HR.Server.Api;

public class AppModules : IAppModule
{
    private static bool IsDevelopment() => AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment();

    public void ConfigureDependencies(IServiceCollection services, IDependencyManager dependencyManager)
    {
        ConfigureServices(services, dependencyManager);

        ConfigureMiddleware(dependencyManager);
    }

    private void ConfigureServices(IServiceCollection services, IDependencyManager dependencyManager)
    {
        dependencyManager.RegisterMinimalDependencies();

        dependencyManager.RegisterDefaultLogger(
#if DEBUG
            typeof(DebugLogStore).GetTypeInfo()
            , typeof(ConsoleLogStore).GetTypeInfo()
#endif
        );

        dependencyManager.RegisterDefaultAspNetCoreApp();

        dependencyManager.RegisterDefaultWebApiAndODataConfiguration();

        var serverAppSettings = AspNetCoreAppEnvironmentsProvider.Current.Configuration.GetSection(nameof(ServerAppSettings)).Get<ServerAppSettings>();
        serverAppSettings.IsDevelopment = IsDevelopment();
        
        services.AddRazorPages().AddApplicationPart(typeof(AppStartup).Assembly);

        services.AddControllers(options =>
        {
            // Add the global MVC Action Filter to the filter chain
            if (AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsProduction())
            {
                
            }

        }).AddApplicationPart(typeof(AppStartup).Assembly);

        services.AddResponseCompression(opts =>
                {
                    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Where(m => m != "text/html").Concat(new[] { "application/octet-stream" }).ToArray();
                    opts.EnableForHttps = true;
                    opts.Providers.Add<BrotliCompressionProvider>();
                    opts.Providers.Add<GzipCompressionProvider>();
                })
                    .Configure<BrotliCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest)
                    .Configure<GzipCompressionProviderOptions>(opt => opt.Level = CompressionLevel.Fastest);

        // Configure Dependencies with Service Installers 
        var assemblies = new[] { Assembly.GetExecutingAssembly() };
        var installers = assemblies.SelectMany(a => a.GetExportedTypes())
            .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic && typeof(IServerInstaller).IsAssignableFrom(c))
            .Select(Activator.CreateInstance).Cast<IServerInstaller>().ToList();
        installers.ForEach(i => i.InstallServices(services, dependencyManager, serverAppSettings));
    }

    private void ConfigureMiddleware(IDependencyManager dependencyManager)
    {
        dependencyManager.RegisterAspNetCoreMiddlewareUsing(aspNetCoreApp =>
        {
#if BlazorClient
            if (AspNetCoreAppEnvironmentsProvider.Current.HostingEnvironment.IsDevelopment())
                aspNetCoreApp.UseWebAssemblyDebugging();
            aspNetCoreApp.UseBlazorFrameworkFiles();
#endif

            // Global CORS Policy
            aspNetCoreApp.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            aspNetCoreApp.UseRequestLocalization();

            aspNetCoreApp.UseResponseCompression();
            aspNetCoreApp.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true
                    };
                }
            });

            aspNetCoreApp.UseRouting();
        });

        dependencyManager.RegisterMinimalAspNetCoreMiddlewares();

        dependencyManager.RegisterAspNetCoreSingleSignOnClient();

        dependencyManager.RegisterODataMiddleware(odataDependencyManager =>
        {
            odataDependencyManager.RegisterGlobalWebApiCustomizerUsing(httpConfiguration =>
            {
                httpConfiguration.EnableSwagger(c =>
                {
                    var xmlDocs = new[] { Assembly.GetExecutingAssembly() }
                        .Select(a => Path.Combine(Path.GetDirectoryName(a.Location)!, $"{a.GetName().Name}.xml"))
                        .Where(File.Exists).ToArray();
                    c.SingleApiVersion("v1", $"Swagger-Api");
                    Array.ForEach(xmlDocs, c.IncludeXmlComments);
                    c.ApplyDefaultODataConfig(httpConfiguration);
                }).EnableBitSwaggerUi();
            });

            odataDependencyManager.RegisterWebApiODataMiddlewareUsingDefaultConfiguration();
        });

        dependencyManager.RegisterAspNetCoreMiddlewareUsing(aspNetCoreApp =>
        {
#if BlazorClient
            #region Route Checkings

            //aspNetCoreApp.MapWhen(context => context.Request.Path == "/", innerAspNetCoreApp =>
            //{
            //    innerAspNetCoreApp.Run(async context =>
            //    {
            //        #region Route base Checks

            //        //if (context.Request.Path == "/")
            //        //{
            //        //    //IUserInformationProvider userInformationProvider = context.RequestServices.GetRequiredService<IUserInformationProvider>();

            //        //    //if (!userInformationProvider.IsAuthenticated())
            //        //    //{
            //        //    //    context.Response.Redirect("/login");
            //        //    //}

            //        //    #region Redirect to some page based on user claims
            //        //    //else
            //        //    //{
            //        //    //    BitJwtToken jwtToken = userInformationProvider.GetBitJwtToken();

            //        //    //    if (jwtToken.Claims.ContainsKey(RedemptionStrings.BusinessId))
            //        //    //    {
            //        //    //        context.Response.Redirect("/home");
            //        //    //    }
            //        //    //    else
            //        //    //    {
            //        //    //        context.Response.Redirect("/select-business");
            //        //    //    }
            //        //    //} 
            //        //    #endregion
            //        //}

            //        #endregion
            //    });
            //}); 

            #endregion
#endif

            aspNetCoreApp.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
#if BlazorClient
                endpoints.MapFallbackToPage("/_Host");
#endif
            });
        }, MiddlewarePosition.AfterOwinMiddlewares);

        // For Telerik file uploads ajax request in case of blazor server && 3rd party Swagger UI!
        dependencyManager.RegisterOwinMiddlewareUsing(owinApp =>
        {
            owinApp.UseCors(CorsOptions.AllowAll);
        });
    }
}

public static class MiddlewareExtension
{
    public static string? GetClientIp(this HttpContext context)
    {
        // Get external network call's IP address [Behind WAF]
        string? ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(ip)) //Internal network
        {
            // Get internal network call's IP address
            ip = context.Connection.RemoteIpAddress?.ToString();
        }

        return ip;
    }
}