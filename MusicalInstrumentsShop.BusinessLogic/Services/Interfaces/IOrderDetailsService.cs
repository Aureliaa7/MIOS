using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        Task AddAsync(OrderDetailsDto orderDetails);
        Task CancelAsync(long orderDetailsId);
        Task<IEnumerable<OrderDetailsDto>> GetAllAsync(Guid userId);
        Task<OrderDetailsDto> GetByIdAsync(long id);
        Task<IEnumerable<OrderDetailsDto>> GetByStatusAsync(Guid userId, OrderStatus status);
    }
}
