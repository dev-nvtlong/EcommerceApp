using AutoMapper;
using EcommerceApp.Application.DTOs.Notification;
using EcommerceApp.Models;

namespace EcommerceApp.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}
