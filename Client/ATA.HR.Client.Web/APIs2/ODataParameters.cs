using Refit;

namespace ATA.HR.Client.Web.APIs;

public class ODataParameters
{
    [Query("$count")]
    public string Count => _count ? "true" : null;

    [Query("$top")]
    public int? Top { get; }

    [Query("$skip")]
    public int? Skip { get; }

    [Query("$filter")]
    public string Filter { get; }

    [Query("$select")]
    public string Select { get; }

    [Query("$orderBy")]
    public string OrderBy { get; }

    private readonly bool _count;

    public ODataParameters(bool count = true, int? top = null, int? skip = null, string filter = null,
        string select = null, string orderBy = null)
    {
        _count = count;
        Top = top;
        Skip = skip;
        Filter = filter;
        Select = select;
        OrderBy = orderBy;
    }
}