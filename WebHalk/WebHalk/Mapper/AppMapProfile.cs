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

            CreateMap<ProductEntity, ProductEditViewModel>()
                .ForMember(x => x.Images, 
                    opt => opt.MapFrom(src => src.ProductImages.Select(pi => new ProductImageViewModel
                    {
                        Id = pi.Id,
                        Name = "/images/"+pi.Image,
                        Priority = pi.Priotity
                    }).ToList()));

            CreateMap<ProductEditViewModel, ProductEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
