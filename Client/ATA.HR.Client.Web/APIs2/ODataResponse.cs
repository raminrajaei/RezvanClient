using System.Text.Json.Serialization;

namespace ATA.HR.Client.Web.APIs;

public class ODataResponse<T>
{
    [JsonPropertyName("@odata.context")]
    public string ODataContext { get; set; }

    /// <summary>
    /// It can be requested by $count=true in query string of your request.
    /// </summary>
    [JsonPropertyName("@odata.count")]
    public int TotalCount { get; set; }

    public List<T> Value { get; set; }
}