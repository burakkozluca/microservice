using Services.Identity.Data.Entities;
using Services.Identity.Models.Auth;

namespace Services.Identity.Services.Auth;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(User user);
}