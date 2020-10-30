using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ISupplierService
    {
        Task Add(SupplierDto supplier);
        Task<IEnumerable<SupplierDto>> GetAll();
        Task<SupplierDto> GetById(Guid id);
        Task Delete(Guid id);
        Task Update(SupplierDto supplier);
        Task<SupplierDto> GetByProduct(string productId);
    }
}
