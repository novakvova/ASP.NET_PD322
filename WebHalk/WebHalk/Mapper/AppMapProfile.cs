using AutoMapper;
using System.Globalization;
using WebHalk.Data.Entities;
using WebHalk.Data.Entities.Identity;
using WebHalk.Models.Account;
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
                    }).ToList()))
                .ForMember(x => x.Price, opt => opt.MapFrom(x => x.Price.ToString(new CultureInfo("uk-UA"))));

            CreateMap<ProductEditViewModel, ProductEntity>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x=>x.Price, opt=>opt.MapFrom(x=>Decimal.Parse(x.Price, new CultureInfo("uk-UA"))));

            CreateMap<UserEntity, ProfileViewModel>()
                .ForMember(x=>x.FullName, opt=>opt.MapFrom(x=>$"{x.LastName} {x.FirstName}"));
        }
    }
}
