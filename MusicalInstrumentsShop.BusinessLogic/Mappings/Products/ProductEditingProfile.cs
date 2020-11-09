using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings.Products
{
    public class ProductEditingProfile : Profile
    {
        public ProductEditingProfile()
        {
            CreateMap<ProductEditingDto, Product>()
                .ForMember(destination => destination.Category, source => source.Ignore())
                .ReverseMap();
        }
    }
}
