using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Cart> GetByUserId(Guid userId)
        {
            return await Context.Set<Cart>()
                .Where(x => x.Customer.Id == userId)
                .FirstAsync();
        }
    }
}
