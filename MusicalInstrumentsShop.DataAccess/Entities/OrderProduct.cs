using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class OrderProduct
    {
        public Guid Id { get; set; }
        public ApplicationUser Customer { get; set; }
        public Product Product { get; set; }
        public int NumberOfProducts { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
