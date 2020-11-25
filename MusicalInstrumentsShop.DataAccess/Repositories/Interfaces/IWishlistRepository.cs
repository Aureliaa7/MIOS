using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<Wishlist> GetByUserId(Guid userId);
    }
}
