using System;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
    }
}
