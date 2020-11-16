using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IDeliveryService
    {
        Task AddAsync(DeliveryMethodDto deliveryMethod);
        Task<IEnumerable<DeliveryMethodDto>> GetAllAsync();
        Task<DeliveryMethodDto> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
