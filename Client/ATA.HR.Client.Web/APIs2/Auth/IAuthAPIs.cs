using ATABit.Shared.Dto.Identity;
using Refit;

namespace ATA.HR.Client.Web.APIs.Auth;

public interface IAuthAPIs
{
    [Post("/auth/api/login")]
    Task<LoginResultDto> Auth_Login([Body] ATALoginDto login);
}