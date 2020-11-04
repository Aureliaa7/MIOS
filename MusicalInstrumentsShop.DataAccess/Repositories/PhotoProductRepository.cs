using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class PhotoProductRepository : Repository<PhotoProduct>, IPhotoProductRepository
    {
        public PhotoProductRepository(ApplicationDbContext context) : base(context) {}

        public async Task<IEnumerable<Photo>> GetByProductId(string id)
        {
            return await Context.Set<PhotoProduct>()
                .Where(x => x.Product.Id == id)
                .Select(x => x.Photo)
                .ToListAsync();
        }

        public async Task DeleteByProductId(string id)
        {
            var photoProducts = await Context.Set<PhotoProduct>()
                .Where(x => x.Product.Id == id)
                .Include(x => x.Photo)
                .ToArrayAsync();

            if (photoProducts != null)
            {
                Context.Set<PhotoProduct>().RemoveRange(photoProducts);
            }
        }
    }
}
