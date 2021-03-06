﻿using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IStockService
    {
        Task AddProductsInStockAsync(StockDto stockDto);
        Task<bool> DecreaseNumberOfProductsAsync(int decreaseBy, string productCode);
        Task<bool> CanTakeAsync(int noProducts, string productCode);
        Task<StockDto> GetByProductAsync(string productId);
        Task IncreaseNumberOfProducts(string productId, int noProducts);
    }
}
