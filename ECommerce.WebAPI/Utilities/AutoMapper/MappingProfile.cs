using AutoMapper;
using ECommerce.Entity.DTO;
using ECommerce.Entity.Models;

namespace ECommerce.WebAPI.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDtoForUpdate, Product>().ReverseMap();

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.Categories, opt => opt.MapFrom(p => p.ProductCategories.Select(pc => pc.Category).ToList()));

            CreateMap<ProductDtoForInsertion, Product>();

            CreateMap<UserForRegistrationDto, User>();

            CreateMap<Category, CategoryDto>();
        }
    }
}
