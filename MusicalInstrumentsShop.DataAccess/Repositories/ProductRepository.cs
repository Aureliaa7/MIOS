using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<string>> Delete(string id)
        {
            var photoNames = await GetPhotoNames(id);

            var productToBeDeleted = await Context.Set<Product>().Where(x => x.Id == id).FirstAsync();
            Context.Set<Product>().Remove(productToBeDeleted);
            await Context.SaveChangesAsync();

            return photoNames;
        }

        public async Task<Product> GetWithRelatedData(string id)
        {
            return await Context.Set<Product>()
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .FirstAsync();
        }

        public async Task<IEnumerable<Product>> GetAllWithRelatedData()
        {
            return await Context.Set<Product>()
                .Include(x => x.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetPhotoNames(string productId)
        {
            return await Context.Set<PhotoProduct>()
               .Where(x => x.Product.Id == productId)
               .Select(x => x.Photo.Name)
               .AsNoTracking()
               .ToListAsync();
        }
    }
}
