using ATA.HR.Shared.Localization.Resources.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Localization.Attributes
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute()
        {
            ErrorMessageResourceType = typeof(DataAnnotationStrings);
            ErrorMessageResourceName = nameof(DataAnnotationStrings.RequiredAttribute_ValidationError);
        }

        public LocalizedRequiredAttribute(string dataAnnotationStringsResourceKey)
        {
            ErrorMessageResourceType = typeof(DataAnnotationStrings);
            ErrorMessageResourceName = dataAnnotationStringsResourceKey;
        }
    }
}