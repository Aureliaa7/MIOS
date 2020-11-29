using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface ISupplierService
    {
        Task AddAsync(SupplierDto supplier);
        Task<IEnumerable<SupplierDto>> GetAllAsync();
        Task<SupplierDto> GetByIdAsync(Guid id);
        Task<IEnumerable<string>> DeleteAsync(Guid id);
        Task UpdateAsync(SupplierDto supplier);
        Task<SupplierDto> GetByProductAsync(string productId);
        Task<int> GetNoSuppliersAsync();
    }
}
