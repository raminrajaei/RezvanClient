using Refit;
using System.Net.Http;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATA.HR.Client.Web.APIs.Models.Response;
using ATABit.Shared;

namespace ATA.HR.Client.Web.APIs;

public interface IRezvanAPIs
{
    // This will automatically be populated by Refit if the property exists
    HttpClient Client { get; }

    #region Child
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

    [Get("/api/Child/{childId}/print")]
    Task<ApiResult<ChildDetailDto>> GetChildDetail(long childId);

    [Get("/api/Child/children-items")]
    Task<ApiResult<List<SelectListItemDto>>> GetChildrenItems();
    #endregion

    #region Adult
    [Get("/api/Adult/get-adults")]
    Task<ApiResult<PagedList<AdultOutputDto>>> GetAdultStudents([Query] AdultInputDto query);

    [Post("/api/Adult/{adultId}/remove")]
    Task<ApiResult> DeleteAdult(long adultId);

    [Get("/api/Adult/{adultId}/GetAdultByIdForForm")]
    Task<ApiResult<AdultUpsertDto>> GetAdultByIdForForm(long adultId);

    [Post("/api/Adult/create")]
    Task<ApiResult> CreateAdultStudent([Body] AdultUpsertDto child);

    [Put("/api/Adult")]
    Task<ApiResult> UpdateAdultStudent([Body] AdultUpsertDto child);

    [Get("/api/Adult/adults-items")]
    Task<ApiResult<List<SelectListItemDto>>> GetAdultsItems();

    [Get("/api/Adult/{adultId}/print")]
    Task<ApiResult<AdultDetailDto>> GetAdultDetail(long adultId);
    #endregion

    #region Teacher
    [Get("/api/Teacher/get-teachers")]
    Task<ApiResult<PagedList<TeacherOutputDto>>> GetTeachers([Query] TeacherInputDto query);

    [Post("/api/Teacher/{teacherId}/remove")]
    Task<ApiResult> DeleteTeacher(long teacherId);

    [Get("/api/Teacher/{teacherId}/GetTeacherByIdForForm")]
    Task<ApiResult<TeacherUpsertDto>> GetTeacherByIdForForm(long teacherId);

    [Post("/api/Teacher/create")]
    Task<ApiResult> CreateTeacher([Body] TeacherUpsertDto teacher);

    [Put("/api/Teacher")]
    Task<ApiResult> UpdateTeacher([Body] TeacherUpsertDto teacher);

    [Get("/api/Teacher/teachers-items")]
    Task<ApiResult<List<SelectListItemDto>>> GetTeachersItems();

    [Get("/api/Teacher/{teacherId}/print")]
    Task<ApiResult<TeacherDetailDto>> GetTeacherDetail(long teacherId);
    #endregion

    #region ClassRoom

    [Get("/api/ClassRoom/get-classes")]
    Task<ApiResult<List<ClassRoomDto>>> GetClasses();

    [Get("/api/ClassRoom/get-classes")]
    Task<ApiResult<List<ClassRoomDto>>> GetClasses([Query] int classType);

    [Post("/api/ClassRoom/upsert-class")]
    Task<ApiResult> UpsertClass([Body] ClassRoomUpsertDto classRoom);

    [Post("/api/ClassRoom/{classId}/remove")]
    Task<ApiResult> DeleteClass(long classId);

    #endregion

    #region ChildClass
    [Get("/api/ChildClass/get-children-classes")]
    Task<ApiResult<PagedList<ChildClassOutputDto>>> GetChildrenClasses([Query] ChildClassInputDto query);

    [Post("/api/ChildClass/{id}/remove")]
    Task<ApiResult> DeleteChildClass(long id);

    [Get("/api/ChildClass/{id}/GetChildClassByIdForForm")]
    Task<ApiResult<ChildClassUpsertDto>> GetChildClassByIdForForm(long id);

    [Post("/api/ChildClass/create")]
    Task<ApiResult> CreateChildClass([Body] ChildClassUpsertDto child);

    [Put("/api/ChildClass/update")]
    Task<ApiResult> UpdateChildClass([Body] ChildClassUpsertDto child);
    #endregion

    #region ClassRoom
    [Get("/api/ClassRoom/classes-items")]
    Task<ApiResult<List<SelectListItemDto>>> GetClassRoomItems([Query] bool childClass);
    #endregion

    #region AdultClass
    [Get("/api/AdultClass/get-adults-classes")]
    Task<ApiResult<PagedList<AdultClassOutputDto>>> GetAdultsClasses([Query] AdultClassInputDto query);

    [Post("/api/AdultClass/{id}/remove")]
    Task<ApiResult> DeleteAdultClass(long id);

    [Get("/api/AdultClass/{id}/GetAdultClassByIdForForm")]
    Task<ApiResult<AdultClassUpsertDto>> GetAdultClassByIdForForm(long id);

    [Post("/api/AdultClass/create")]
    Task<ApiResult> CreateAdultClass([Body] AdultClassUpsertDto adult);

    [Put("/api/AdultClass/update")]
    Task<ApiResult> UpdateAdultClass([Body] AdultClassUpsertDto adult);
    #endregion
}