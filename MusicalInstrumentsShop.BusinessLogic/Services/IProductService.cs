using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IProductService
    {
        public Task Add(Product product);
        public Task<IEnumerable<Product>> GetAll();
        public Task<Product> GetById(string id);
        public Task Delete(string id);
        public Task Update(Product product);
    }
}
