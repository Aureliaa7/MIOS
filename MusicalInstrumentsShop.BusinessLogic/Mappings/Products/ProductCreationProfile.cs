using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings.Products
{
    public class ProductCreationProfile : Profile
    {
        public ProductCreationProfile()
        {
            CreateMap<ProductCreationDto, Product>()
                .ForMember(destination => destination.Category, source => source.Ignore());
        }
    }
}
