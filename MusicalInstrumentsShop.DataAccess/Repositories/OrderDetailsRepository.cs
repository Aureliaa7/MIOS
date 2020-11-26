using Microsoft.EntityFrameworkCore;
using MusicalInstrumentsShop.DataAccess.Data;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.DataAccess.Repositories
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<OrderDetails>> GetAllWithRelatedData(Guid customerId)
        {
            return await Context.Set<OrderDetails>()
                .Where(x => x.Customer.Id == customerId)
                .Include(x => x.PaymentMethod)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.Customer)
                .ToListAsync();
        }

        public async Task<OrderDetails> GetByIdWithRelatedData(long id)
        {
            return await Context.Set<OrderDetails>()
                .Where(x => x.Id == id)
                .Include(x => x.PaymentMethod)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.Customer)
                .FirstAsync();
        }

        public async Task<IEnumerable<OrderDetails>> GetByStatusWithRelatedData(Guid customerId, OrderStatus status)
        {
            return await Context.Set<OrderDetails>()
                .Where(x => x.Customer.Id == customerId && x.Status == status)
                .Include(x => x.PaymentMethod)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.Customer)
                .ToListAsync();
        }
    }
}
