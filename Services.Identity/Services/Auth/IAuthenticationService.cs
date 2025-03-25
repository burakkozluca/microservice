using Services.Identity.Models.Auth;
using Services.Identity.Models.Auth.Requests;

namespace Services.Identity.Services.Auth;

public interface IAuthenticationService
{
    Task<ServiceResult<TokenDto>> CreateTokenAsync(LoginDto loginDto);
    Task<ServiceResult<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
    Task<ServiceResult> RevokeRefreshToken(string refreshToken);

    Task<ServiceResult> GeneratePasswordResetTokenAsync(ForgotPasswordRequest request);
    Task<ServiceResult> VerifyPasswordResetTokenAsync(VerifyUserToken request);

}