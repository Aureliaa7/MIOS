using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IWishlistProductService
    {
        Task<IEnumerable<WishlistProductDto>> GetAllAsync(Guid userId);
        Task AddAsync(WishlistProductCreationDto wishlistCreationDto);
        Task DeleteAsync(Guid id);
    }
}
