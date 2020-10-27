using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDbContext context) : base(context) {}

        public async Task<Stock> GetByProductId(string productId)
        {
            return await Context.Set<Stock>()
               .Where(x => x.Product.Id == productId)
               .Include(x => x.Supplier)
               .FirstAsync();
        }

        public async Task<Stock> GetWithRelatedData(Guid id)
        {
            return await Context.Set<Stock>()
                .Where(x => x.Id == id)
                .Include(x => x.Supplier)
                .FirstAsync();
        }
    }
}
