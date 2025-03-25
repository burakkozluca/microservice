using AutoMapper;
using Services.Identity.Data.Entities;
using Services.Identity.Models.User;
using Services.Identity.Models.User.Requests;

namespace Services.Identity.Services.Users;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
         
        CreateMap<UpdateUserRequest, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 
    }
}