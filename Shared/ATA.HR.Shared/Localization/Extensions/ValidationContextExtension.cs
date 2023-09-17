using ATA.HR.Shared.Localization.Contract;
using Bit.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Localization.Extensions
{
    public static class ValidationContextExtension
    {
        public static IStringsProvider GetStringsProvider(this ValidationContext validationContext)
        {
            // Request originated from AspNetCore Controllers
            var stringsProvider = validationContext.GetService(typeof(IStringsProvider));

            if (stringsProvider is not null)
                return (IStringsProvider)stringsProvider;

            // Below block should be uncomment in backend only projects. Blazor will use AspNetCore context and
            // will validate the dto in client-side, so no need to backend error message.

            #region Request originated from Bit Controllers

            //var context = DefaultDependencyManager.Current.Resolve<IHttpContextAccessor>().HttpContext;

            //validationContext.InitializeServiceProvider(type => context!.RequestServices.GetRequiredService(type));

            //return validationContext.GetRequiredService<IStringsProvider>();

            #endregion

            throw new DomainLogicException("In Blazor applications, this error should not happen! See the above comments");
        }
    }
}