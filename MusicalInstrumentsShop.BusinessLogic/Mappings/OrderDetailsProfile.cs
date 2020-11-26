using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings
{
    public class OrderDetailsProfile : Profile
    {
        public OrderDetailsProfile()
        {
            CreateMap<OrderDetails, OrderDetailsDto>()
                .ForMember(destination => destination.CustomerId, source => source.Ignore())
                .ForMember(destination => destination.DeliveryMethodName, source => source.Ignore())
                .ForMember(destination => destination.PaymentMethodName, source => source.Ignore());

            CreateMap<OrderDetailsDto, OrderDetails>()
                .ForMember(destination => destination.Customer, source => source.Ignore())
                .ForMember(destination => destination.DeliveryMethod, source => source.Ignore())
                .ForMember(destination => destination.PaymentMethod, source => source.Ignore());
        }
    }
}
