using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class StockService : IStockService
    {
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IStockRepository stockRepository;

        public StockService(IRepository<Product> productRepository, IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository, IStockRepository stockRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
            this.stockRepository = stockRepository;
        }

        public async Task AddProductsInStock(StockDto stockDto)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == stockDto.CategoryId);
            bool supplierExists = await supplierRepository.Exists(x => x.Id == stockDto.SupplierId);
            bool productExists = await productRepository.Exists(x => x.Id == stockDto.ProductId);
            if(categoryExists && supplierExists && productExists)
            {
                var searchedStock = await stockRepository.GetByProductId(stockDto.ProductId);
                searchedStock.NumberOfProducts += stockDto.NumberOfProducts;
                await stockRepository.Update(searchedStock);
            }
            else
            {
                throw new ItemNotFoundException("The searched entities were not found...");
            }
        }
    }
}
