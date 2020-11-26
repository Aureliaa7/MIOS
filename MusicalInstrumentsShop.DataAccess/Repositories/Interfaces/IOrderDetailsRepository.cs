using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories.Interfaces
{
    public interface IOrderDetailsRepository: IRepository<OrderDetails>
    {
        Task<IEnumerable<OrderDetails>> GetAllWithRelatedData(Guid customerId);
        Task<IEnumerable<OrderDetails>> GetByStatusWithRelatedData(Guid customerId, OrderStatus status);
        Task<OrderDetails> GetByIdWithRelatedData(long id);
    }
}
