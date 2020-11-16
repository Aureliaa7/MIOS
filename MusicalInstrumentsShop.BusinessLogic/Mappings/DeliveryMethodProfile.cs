using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings
{
    public class DeliveryMethodProfile : Profile
    {
        public DeliveryMethodProfile()
        {
            CreateMap<DeliveryMethod, DeliveryMethodDto>().ReverseMap();
        }
    }
}
