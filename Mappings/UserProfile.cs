using AutoMapper;
using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UpdateProfileDto, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
