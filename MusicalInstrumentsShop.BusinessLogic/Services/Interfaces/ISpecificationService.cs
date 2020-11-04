using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface ISpecificationService
    {
        Task AddAsync(SpecificationDto specificationDto);
        Task<SpecificationDto> GetByIdAsync(Guid id);
        Task<IEnumerable<SpecificationDto>> GetForProductAsync(string productId);
        Task DeleteAsync(Guid id);
    }
}
