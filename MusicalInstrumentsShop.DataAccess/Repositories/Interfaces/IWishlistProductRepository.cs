using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IWishlistProductRepository : IRepository<WishlistProduct>
    {
        Task<IEnumerable<WishlistProduct>> GetByUserId(Guid userId);
    }
}
