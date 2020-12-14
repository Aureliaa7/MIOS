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
    public class CartProductRepository : Repository<CartProduct>, ICartProductRepository
    {
        public CartProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<CartProduct>> GetByCartId(Guid cartId)
        {
            return await Context.Set<CartProduct>()
               .Where(x => x.Cart.Id == cartId)
               .Include(x => x.Product)
                   .ThenInclude(x => x.Category)
               .Include(x => x.Cart)
                   .ThenInclude(x => x.Customer)
               .ToListAsync();
        }

        public async Task<CartProduct> GetById(Guid id)
        {
            return await Context.Set<CartProduct>()
                .Where(x => x.Id == id)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Cart)
                    .ThenInclude(x => x.Customer)
                .FirstAsync();
        }

        public async Task<CartProduct> GetByProduct(string productId)
        {
            return await Context.Set<CartProduct>()
                .Where(x => x.Product.Id == productId)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Cart)
                    .ThenInclude(x => x.Customer)
                .FirstAsync();
        }

        public async Task<IEnumerable<CartProduct>> GetByUserId(Guid userId)
        {
            return await Context.Set<CartProduct>()
                .Where(x => x.Cart.Customer.Id == userId)
                .Include(x => x.Product)
                    .ThenInclude(x => x.Category)
                .Include(x => x.Cart)
                    .ThenInclude(x => x.Customer)
                .ToListAsync();
        }
    }
}
