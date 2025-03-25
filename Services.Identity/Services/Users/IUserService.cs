using Microsoft.AspNetCore.Identity.Data;
using Services.Identity.Models.User;
using Services.Identity.Models.User.Requests;

namespace Services.Identity.Services.Users;

public interface IUserService
{
    Task<ServiceResult> CreateUserAsync(CreateUserRequest request);
    Task<ServiceResult<UserDto>> GetUserByName(string userName);
    Task<ServiceResult> ChangePassword(UpdatePasswordRequest request , string UserId);
    Task<ServiceResult<UserDto>> UpdateUserAsync(UpdateUserRequest request, string userId);

    Task<ServiceResult> ResetPassword(ResetPasswordRequest request);
}