using AutoMapper;
using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        { 
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName))
                .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ShipName, opt => opt.MapFrom(src => src.ShipName ?? (src.User.FullName ?? src.User.UserName)))
                .ForMember(dest => dest.ShipAddress, opt => opt.MapFrom(src => src.ShipAddress ?? src.User.Address))
                .ForMember(dest => dest.ShipPhone, opt => opt.MapFrom(src => src.ShipPhone ?? src.User.PhoneNumber));
            CreateMap<CreateOrderDto, Order>();
            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
