using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IRepository<Product> productRepository;
        private readonly IStockRepository stockRepository;

        public SupplierService(IRepository<Supplier> supplierRepository, IRepository<Product> productRepository,
            IStockRepository stockRepository)
        {
            this.supplierRepository = supplierRepository;
            this.productRepository = productRepository;
            this.stockRepository = stockRepository;
        }

        public async Task Add(Supplier supplier)
        {
            await supplierRepository.Add(supplier);
        }

        public async Task Delete(Guid id)
        {
            bool supplierExists = await supplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                await supplierRepository.Remove(id);
            }
            else
            {
                throw new ItemNotFoundException("The supplier was not found...");
            }
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await supplierRepository.GetAll();
        }

        public async Task<Supplier> GetById(Guid id)
        {
            bool supplierExists = await supplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                return await supplierRepository.Get(id);
            }
            throw new ItemNotFoundException("The supplier was not found...");
        }

        public async Task<Supplier> GetByProduct(string productId)
        {
            bool productExists = await productRepository.Exists(x => x.Id == productId);
            if (productExists)
            {
                var stock = await stockRepository.GetByProductId(productId);
                if(stock != null)
                {
                    return stock.Supplier;
                }
            }
            throw new ItemNotFoundException("The product was not found");
        }

        public async Task Update(Supplier supplier)
        {
            await supplierRepository.Update(supplier);
        }
    }
}
