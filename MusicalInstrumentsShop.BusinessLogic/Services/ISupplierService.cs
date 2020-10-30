using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ISupplierService
    {
        Task Add(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAll();
        Task<Supplier> GetById(Guid id);
        Task Delete(Guid id);
        Task Update(Supplier supplier);
        Task<Supplier> GetByProduct(string productId);
    }
}
