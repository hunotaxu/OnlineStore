using AutoMapper;
using DAL.Data.Entities;
using OnlineStore.Models.ViewModels.Item;
using Utilities.DTOs;

namespace OnlineStore.MapperProfiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemViewModel>();
            CreateMap<PagedResult<Item>, PagedResult<ItemViewModel>>();
        }
    }
}