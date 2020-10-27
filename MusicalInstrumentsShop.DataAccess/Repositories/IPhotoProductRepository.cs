using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public interface IPhotoProductRepository : IRepository<PhotoProduct>
    {
        public Task<IEnumerable<Photo>> GetByProductId(string id);
        public Task DeleteByProductId(string id);
    }
}
