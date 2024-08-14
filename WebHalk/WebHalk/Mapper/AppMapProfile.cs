using AutoMapper;
using WebHalk.Data.Entities;
using WebHalk.Models.Categories;

namespace WebHalk.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryEntity, CategoryEditViewModel>();
        }
    }
}
