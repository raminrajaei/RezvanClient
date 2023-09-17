using ATA.HR.Server.Api;
using Bit.Core.Contracts;
using Bit.OData.Contracts;
using Bit.Owin;

[assembly: ODataModule("Rezvan")]
[assembly: AppModule(typeof(AppModules))]

namespace ATA.HR.Server.Api;

public class AppStartup : AspNetCoreAppStartup
{
}