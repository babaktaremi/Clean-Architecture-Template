using System.ComponentModel.DataAnnotations;

namespace Application.Models.ApiResult
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Success")]
        Success = 200,

        [Display(Name = "Server Error")]
        ServerError = 500,

        [Display(Name = "Bad Request Error")]
        BadRequest = 400,

        [Display(Name = "Not Found")]
        NotFound = 404,

        [Display(Name = "Empty Error")]
        ListEmpty = 404,

        [Display(Name = "Process Error")]
        LogicError = 500,

        [Display(Name = "Authentication Error")]
        UnAuthorized = 401,

        [Display(Name = "Not Acceptable")]
        NotAcceptable = 406,

        [Display(Name = "Failed Dependency")]
        FailedDependency = 424
    }
}
