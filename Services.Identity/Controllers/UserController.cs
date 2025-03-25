using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Services.Identity.Models.User.Requests;
using Services.Identity.Services.Users;

namespace Services.Identity.Controllers;

public class UserController(IUserService userService) : CustomController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest userDto)
    {
        var result = await userService.CreateUserAsync(userDto);
        return CreateActionResult(result);
    }



    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var result = await userService.GetUserByName(HttpContext.User.Identity.Name);
        return CreateActionResult(result);
    }


    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(UpdatePasswordRequest request)
    {
        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var result = await userService.ChangePassword(request , userId);
        return CreateActionResult(result);
    }

    [HttpPost("UpdateProfile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UpdateUserRequest request)
    {
        string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

        var result = await userService.UpdateUserAsync(request, userId);
        return CreateActionResult(result);
    }

    [HttpPost("ResetPassword")]
 
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {          
        var result = await userService.ResetPassword(request);
        return CreateActionResult(result);
    }
}