using AutoMapper;
using EcommerceApp.Application.DTOs.Category;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
