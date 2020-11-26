using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IOrderProductRepository : IRepository<OrderProduct>
    {
        Task<IEnumerable<OrderProduct>> GetByOrderDetailsId(long id);
    }
}
