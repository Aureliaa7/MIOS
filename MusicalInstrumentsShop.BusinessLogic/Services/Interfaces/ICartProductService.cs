using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface ICartProductService
    {
        Task<IEnumerable<CartProductDto>> GetAllAsync(Guid userId);
        Task AddAsync(CartProductCreationDto cartProductCreationDto);
        Task DeleteAsync(Guid id);
        Task<bool> UpdateNumberOfProductsAsync(string productCode, int numberOfProducts);
        Task EmptyCart(Guid cartId);
    }
}
