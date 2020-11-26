using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public class OrderProductRepository : Repository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDbContext context): base(context) { }

        public async Task<IEnumerable<OrderProduct>> GetByOrderDetailsId(long id)
        {
            return await Context.Set<OrderProduct>()
                .Where(x => x.OrderDetails.Id == id)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .ToListAsync();
        }
    }
}
