using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetWithRelatedDataAsTracking(string id);
        Task<Product> GetWithRelatedDataAsNoTracking(string id);
        Task<IEnumerable<string>> Delete(string id);
        Task<IEnumerable<Product>> GetAllWithRelatedData();
        Task<IEnumerable<string>> GetPhotoNames(string productId);
    }
}
