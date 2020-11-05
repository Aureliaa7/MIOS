using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork unitOfWork;

        public StockService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddProductsInStockAsync(StockDto stockDto)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == stockDto.CategoryId);
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == stockDto.SupplierId);
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == stockDto.ProductId);
            if(categoryExists && supplierExists && productExists)
            {
                var searchedStock = await unitOfWork.StockRepository.GetByProductId(stockDto.ProductId);
                searchedStock.NumberOfProducts += stockDto.NumberOfProducts;
                await unitOfWork.StockRepository.Update(searchedStock);
            }
            else
            {
                throw new ItemNotFoundException("The searched entities were not found...");
            }
        }
    }
}
