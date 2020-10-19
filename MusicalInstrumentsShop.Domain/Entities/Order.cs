using System;
using System.Collections.Generic;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public Shipping Shipping { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public List<Product> Product { get; set; }
    }
}
