using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
