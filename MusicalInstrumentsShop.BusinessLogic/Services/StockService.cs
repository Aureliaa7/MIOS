using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using AutoMapper;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StockService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddProductsInStockAsync(StockDto stockDto)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == stockDto.CategoryId);
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == stockDto.SupplierId);
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == stockDto.ProductId);
            if (categoryExists && supplierExists && productExists)
            {
                var searchedStock = await unitOfWork.StockRepository.GetByProductId(stockDto.ProductId);
                searchedStock.NumberOfProducts += stockDto.NumberOfProducts;
                unitOfWork.StockRepository.Update(searchedStock);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The searched entities were not found...");
            }
        }

        public async Task<bool> CanTakeAsync(int noProducts, string productCode)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productCode);
            if (productExists)
            {
                var stock = await unitOfWork.StockRepository.GetByProductId(productCode);
                if (stock != null && (stock.NumberOfProducts - noProducts >= 0))
                {
                    return true;
                }
                return false;
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<bool> DecreaseNumberOfProductsAsync(int decreaseBy, string productCode)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productCode);
            if (productExists)
            {
                var stock = await unitOfWork.StockRepository.GetByProductId(productCode);
                if (stock != null && decreaseBy <= stock.NumberOfProducts)
                {
                    stock.NumberOfProducts -= decreaseBy;
                    unitOfWork.StockRepository.Update(stock);
                    await unitOfWork.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<StockDto> GetByProductAsync(string productId)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productId);
            if(productExists)
            {
                var stock = await unitOfWork.StockRepository.GetByProductId(productId);
                return mapper.Map<StockDto>(stock);
            }
            throw new ItemNotFoundException("The product was not found...");
        }
    }
}
