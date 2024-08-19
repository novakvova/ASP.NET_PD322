using AutoMapper;
using WebHalk.Data.Entities;
using WebHalk.Models.Categories;
using WebHalk.Models.Products;

namespace WebHalk.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<CategoryEntity, CategoryItemViewModel>();
            CreateMap<CategoryEntity, CategoryEditViewModel>();
            CreateMap<ProductEntity, ProductItemViewModel>()
                .ForMember(x=>x.Images, opt=>opt.MapFrom(x=>x.ProductImages.Select(p=>p.Image).ToArray()));
        }
    }
}
