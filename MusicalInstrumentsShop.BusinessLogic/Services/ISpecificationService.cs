using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ISpecificationService
    {
        Task Add(SpecificationDto specificationDto);
        Task<SpecificationDto> GetById(Guid id);
        Task<IEnumerable<SpecificationDto>> GetForProduct(string productId);
        Task Delete(Guid id);
    }
}
