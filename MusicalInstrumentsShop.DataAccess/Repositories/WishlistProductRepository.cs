using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class WishlistProductRepository : Repository<WishlistProduct>, IWishlistProductRepository
    {
        public WishlistProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<WishlistProduct>> GetByUserId(Guid userId)
        {
            return await Context.Set<WishlistProduct>()
                .Where(x => x.Wishlist.User.Id == userId)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Wishlist)
                    .ThenInclude(x => x.User)
                .ToListAsync();
        }
    }
}
