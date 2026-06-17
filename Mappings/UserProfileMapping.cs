using AutoMapper;
using EcommerceApp.Application.DTOs.Account;
using EcommerceApp.Enums;
using EcommerceApp.Models;
using EcommerceApp.Models.Entities;

namespace EcommerceApp.Mappings
{
    public class UserProfileMapping : Profile
    {
        public UserProfileMapping()
        {
            CreateMap<UserProfile, UserDto>()

                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        src.FullName != null
                        ? src.FullName
                        : null))

                .ForMember(
                    dest => dest.Avatar,
                    opt => opt.MapFrom(src =>
                        src.AvatarUrl != null
                        ? src.AvatarUrl
                        : null))

                .ForMember(
                    dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src =>
                        src.PhoneNumber != null
                        ? src.PhoneNumber
                        : null))

                .ForMember(
                    dest => dest.Gender,
                    opt => opt.MapFrom(src =>
                        src != null
                        ? src.Gender
                        : GenderType.Other))

                .ForMember(
                    dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src =>
                        src != null && src.DateOfBirth.HasValue
                            ? src.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
                            : (DateTime?)null))

                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src =>
                        src != null
                        ? src.Email
                        : null));

            // Mapping User -> UserDto (dùng cho GetAllUsersAsync)
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.Profile != null ? src.Profile.FullName : src.UserName ?? src.Email))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src =>
                    src.Profile != null ? src.Profile.AvatarUrl : null))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    src.Profile != null ? src.Profile.PhoneNumber : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src =>
                    src.Profile != null ? src.Profile.Gender : GenderType.Other))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                    src.Profile != null && src.Profile.DateOfBirth.HasValue
                        ? src.Profile.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                    src.UserRoles.Select(ur => ur.Role.Name).ToList()));
        }
    }
}
