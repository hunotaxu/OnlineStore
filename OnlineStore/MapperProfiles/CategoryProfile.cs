using AutoMapper;
using DAL.Data.Entities;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.MapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>();
        }
    }
}
