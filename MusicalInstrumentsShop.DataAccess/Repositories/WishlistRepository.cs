using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Wishlist> GetByUserId(Guid userId)
        {
            return await Context.Set<Wishlist>()
               .Where(x => x.User.Id == userId)
               .Include(x => x.User)
               .FirstAsync();
        }
    }
}
