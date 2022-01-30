using AutoMapper;
using DAL.Data.Entities;
using OnlineStore.Models.ViewModels;
using Utilities.DTOs;

namespace OnlineStore.MapperProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderInfoViewModel>();
            CreateMap<Order, OrderDeliveryInfoViewModel>();
            CreateMap<PagedResult<Order>, PagedResult<OrderInfoViewModel>>();
        }
    }
}
