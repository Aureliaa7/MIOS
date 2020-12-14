using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface ICartProductRepository : IRepository<CartProduct>
    {
        Task<IEnumerable<CartProduct>> GetByUserId(Guid userId);
        Task<CartProduct> GetByProduct(string productId);
        Task<CartProduct> GetById(Guid id);
        Task<IEnumerable<CartProduct>> GetByCartId(Guid cartId);
    }
}
