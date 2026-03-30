using AutoMapper;
using EcommerceApp.Application.DTOs.Cart;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
        }
    }
}
