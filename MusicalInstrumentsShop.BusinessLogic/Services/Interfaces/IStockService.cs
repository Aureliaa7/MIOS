using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IStockService
    {
        Task AddProductsInStockAsync(StockDto stockDto);
    }
}
