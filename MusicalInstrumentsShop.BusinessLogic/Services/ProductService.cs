using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task Add(Product product)
        {
            await productRepository.Add(product);
        }

        public async Task Delete(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                await productRepository.Delete(id);
            }
            else
            {
                throw new ItemNotFoundException("The product was not found...");
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await productRepository.GetAllWithRelatedData();
        }

        public async Task<Product> GetById(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                return await productRepository.GetWithRelatedData(id);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task Update(Product product)
        {
            await productRepository.Update(product);
        }
    }
}
