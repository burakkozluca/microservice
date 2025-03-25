using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Services.Identity.Data.Entities;
using Services.Identity.Models.User;
using Services.Identity.Models.User.Requests;

namespace Services.Identity.Services.Users;

public class UserService
    (
        IMapper mapper,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    ) : IUserService
    {

        public async Task<ServiceResult<UserDto>> UpdateUserAsync(UpdateUserRequest request ,  string userId)
        {

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ServiceResult<UserDto>.Fail("User not found");
            }

            var updatedUser = mapper.Map(request,user);
            

            var result = await userManager.UpdateAsync(updatedUser);

            if (!result.Succeeded)
            {
                //bu mekanızma zaten girilen değerleri kontrol ediyor o yüzden validatora gerek olmayabilir
                //tel email 
                //daha sonra eklenecek
                var errors = result.Errors.Select(x => x.Description).ToList();

                return ServiceResult<UserDto>.Fail(errors);
            }


            return ServiceResult<UserDto>.Success(mapper.Map<UserDto>(updatedUser));
        }

        public async Task<ServiceResult> CreateUserAsync(CreateUserRequest request)
        {
            var user = new User { Email = request.email, UserName = request.Username };

            var result = await userManager.CreateAsync(user, request.password);
            if (!result.Succeeded)
            {
                //bu mekanızma zaten girilen değerleri kontrol ediyor o yüzden validatora gerek olmayabilir
                var errors = result.Errors.Select(x => x.Description).ToList();

                return ServiceResult.Fail(errors);
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult<UserDto>> GetUserByName(string userName)
        {

            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return ServiceResult<UserDto>.Fail("User not found");
            }

            return ServiceResult<UserDto>.Success(mapper.Map<UserDto>(user));
        }

        public async Task<ServiceResult> ChangePassword(UpdatePasswordRequest request ,  string UserId)
        {
           
            var user = await userManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return ServiceResult.Fail("User not found");
            }

            var checkUserPassword = await userManager.CheckPasswordAsync(user, request.OldPassword);

            if(!checkUserPassword) return ServiceResult.Fail("Password is wrong");

            var resultUpdate = await userManager.ChangePasswordAsync(user,request.OldPassword , request.NewPassword);

            if (!resultUpdate.Succeeded)
            {
                var errors = resultUpdate.Errors.Select(x => x.Description).ToList();

                return ServiceResult.Fail(errors);
            }
            return ServiceResult.Success();
        }

        public async Task<ServiceResult> ResetPassword(ResetPasswordRequest request)
        {

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return ServiceResult.Fail("User not found");
            }

            

            var resultUpdate = await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);

            if (!resultUpdate.Succeeded)
            {
                var errors = resultUpdate.Errors.Select(x => x.Description).ToList();

                return ServiceResult.Fail(errors);
            }
            return ServiceResult.Success();
        }


    }
