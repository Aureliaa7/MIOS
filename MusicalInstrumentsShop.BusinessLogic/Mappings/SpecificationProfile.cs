using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Mappings
{
    public class SpecificationProfile : Profile
    {
        public SpecificationProfile()
        {
            CreateMap<Specification, SpecificationDto>();

            CreateMap<SpecificationDto, Specification>()
                .ForMember(destination => destination.Product, source => source.Ignore());
        }
    }
}
