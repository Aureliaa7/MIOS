using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithRelatedData(string id);
        Task<IEnumerable<string>> Delete(string id);
        Task<IEnumerable<Product>> GetAllWithRelatedData();
        Task<IEnumerable<string>> GetPhotoNames(string productId);
    }
}
