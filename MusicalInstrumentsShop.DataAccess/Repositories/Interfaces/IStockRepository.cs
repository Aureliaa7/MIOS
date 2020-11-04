using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task<Stock> GetWithRelatedData(Guid id);
        Task<Stock> GetByProductId(string id);
        Task<IEnumerable<Stock>> GetBySupplierId(Guid id);
    }
}
