using AutoMapper;

namespace Server.Dtos.Product
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Models.Product, ProductDto>();
            CreateMap<Models.Product, ProductItemResponse>();
        }
    }
}
