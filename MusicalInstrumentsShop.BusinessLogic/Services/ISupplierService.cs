using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ISupplierService
    {
        public Task Add(Supplier supplier);
        public Task<IEnumerable<Supplier>> GetAll();
        public Task<Supplier> GetById(Guid id);
        public Task Delete(Guid id);
        public Task Update(Supplier supplier);
    }
}
