using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IStockService
    {
        Task AddProductsInStock(StockDto stockDto);
    }
}
