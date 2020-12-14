using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetByUserId(Guid userId);
    }
}
