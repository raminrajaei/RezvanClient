namespace ATA.HR.Client.Web.APIs.Models.Response;

public class ApiResult<TData> : ApiResult
{
    public TData Data { get; set; }
}

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
}