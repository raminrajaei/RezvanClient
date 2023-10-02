using ATABit.Shared;
using DNTPersianUtils.Core;

namespace ATA.HR.Client.Web.Extensions;

public static class BusinessDateFilterExtensions
{
    public static List<SelectListItem> GetYears(this List<SelectListItem> items)
    {
        var endDate = DateTime.Now.AddYears(2);
        var startDate = new DateTime(2021, 01, 01);
            
        for (var dt = startDate; dt <= endDate; dt = dt.AddYears(1))
        {
            var year = dt.GetPersianYear();

            items.Add(new SelectListItem
            {
                Value = year.ToString(),
                Text = year.ToString()
            });
        }

        return items;
    }
}