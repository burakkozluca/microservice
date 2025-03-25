using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Identity.Data;
using Services.Identity.Data.Entities;
using Services.Identity.Models.Auth;
using Services.Identity.Models.Auth.Requests;

namespace Services.Identity.Services.Auth;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;

    public AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService,
        AppDbContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ServiceResult<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.email);

        if (user == null) 
            return ServiceResult<TokenDto>.Fail("Email veya şifreye ait kullanıcı bulunamadı", HttpStatusCode.NotFound);

        var userPasswordCheck = await _userManager.CheckPasswordAsync(user, loginDto.password);

        if (!userPasswordCheck) 
            return ServiceResult<TokenDto>.Fail("Email veya şifreye ait kullanıcı bulunamadı", HttpStatusCode.NotFound);

        var token = await _tokenService.CreateTokenAsync(user);

        // UserRefreshToken işlemleri
        var existingRefreshToken = await _context.Set<UserRefreshToken>()
            .FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (existingRefreshToken == null)
        {
            var newRefreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                Code = token.RefreshToken,
                Expiration = token.RefreshTokenExpiration
            };
            _context.Set<UserRefreshToken>().Add(newRefreshToken);
        }
        else
        {
            existingRefreshToken.Code = token.RefreshToken;
            existingRefreshToken.Expiration = token.RefreshTokenExpiration;
            _context.Set<UserRefreshToken>().Update(existingRefreshToken);
        }

        await _context.SaveChangesAsync();

        return ServiceResult<TokenDto>.Success(token);
    }

    public async Task<ServiceResult<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
    {
        var existingRefreshToken = await _context.Set<UserRefreshToken>()
            .FirstOrDefaultAsync(x => x.Code == refreshToken);

        if (existingRefreshToken == null) 
            return ServiceResult<TokenDto>.Fail("Token bulunamadı", HttpStatusCode.NotFound);

        if (existingRefreshToken.Expiration <= DateTime.Now) 
            return ServiceResult<TokenDto>.Fail("Refresh token has expired", HttpStatusCode.Unauthorized);

        var user = await _userManager.FindByIdAsync(existingRefreshToken.UserId);

        if (user == null) 
            return ServiceResult<TokenDto>.Fail("Kullanıcı bulunamadı", HttpStatusCode.NotFound);

        var token = await _tokenService.CreateTokenAsync(user);

        existingRefreshToken.Code = token.RefreshToken;
        existingRefreshToken.Expiration = token.RefreshTokenExpiration;
        
        await _context.SaveChangesAsync();

        return ServiceResult<TokenDto>.Success(token);
    }

    public async Task<ServiceResult> RevokeRefreshToken(string refreshToken)
    {
        var existingRefreshToken = await _context.Set<UserRefreshToken>()
            .FirstOrDefaultAsync(x => x.Code == refreshToken);

        if (existingRefreshToken == null) 
            return ServiceResult.Fail("Token bulunamadı", HttpStatusCode.NotFound);

        _context.Set<UserRefreshToken>().Remove(existingRefreshToken);
        await _context.SaveChangesAsync();

        return ServiceResult.Success(HttpStatusCode.NoContent);
    }

    public async Task<ServiceResult> GeneratePasswordResetTokenAsync(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.email);

        if (user == null)
            return ServiceResult.Fail("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    
        // Token üretildi ise başarılı sonuç dön
        if (!string.IsNullOrEmpty(token))
            return ServiceResult.Success();
    
        return ServiceResult.Fail("Token generation failed");
    }

    public async Task<ServiceResult> VerifyPasswordResetTokenAsync(VerifyUserToken request)
    {
        var user = await _userManager.FindByEmailAsync(request.email);

        if (user == null)
            return ServiceResult.Fail("User not found");

        var checkToken = await _userManager.VerifyUserTokenAsync(
            user, 
            _userManager.Options.Tokens.PasswordResetTokenProvider, 
            "ResetPassword", 
            request.token);

        if (!checkToken) 
            return ServiceResult.Fail("Token is expired");

        return ServiceResult.Success();
    }
}