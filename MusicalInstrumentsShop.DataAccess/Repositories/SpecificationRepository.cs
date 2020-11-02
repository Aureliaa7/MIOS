using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class SpecificationRepository : Repository<Specification>, ISpecificationRepository
    {
        public SpecificationRepository(ApplicationDbContext context): base(context) { }

        public async Task<IEnumerable<Specification>> GetByProductId(string id)
        {
            return await Context.Set<Specification>()
                .Where(x => x.Product.Id == id)
                .ToListAsync();
        }
    }
}
