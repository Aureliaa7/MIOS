using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        public Task<Stock> GetWithRelatedData(Guid id);
        public Task<Stock> GetByProductId(string productId);
    }
}
