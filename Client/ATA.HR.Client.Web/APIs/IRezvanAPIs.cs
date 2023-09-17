﻿using Refit;
using System.Net.Http;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.APIs.Models.Response;

namespace ATA.HR.Client.Web.APIs;

public interface IRezvanAPIs
{
    // This will automatically be populated by Refit if the property exists
    HttpClient Client { get; }

    [Get("/api/Child/get-children")]
    Task<ApiResult<PagedList<ChildrenOutputDto>>> GetChildStudents([Query] ChildrenInputDto query);

    [Post("/api/Child/create")]
    Task<ApiResult> CreateChildStudent([Body] ChildUpsertDto child);
    
    [Put("/api/Child")]
    Task<ApiResult> UpdateChildStudent([Body] ChildUpsertDto child);
    
    [Get("/api/Child/{childId}/GetChildByIdForForm")]
    Task<ApiResult<ChildUpsertDto>> GetChildByIdForForm(long childId);
    
    [Post("/api/Child/{childId}/remove")]
    Task<ApiResult> DeleteChild(long childId);
}