using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings
{
    public class StockProfile: Profile
    {
        public StockProfile()
        {
            CreateMap<Stock, StockDto>()
              .ForMember(destination => destination.CategoryId, source => source.Ignore())
              .ForMember(destination => destination.ProductId, source => source.Ignore())
              .ForMember(destination => destination.SupplierId, source => source.Ignore());
        }
    }
}
