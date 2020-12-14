using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class CartProduct
    {
        public Guid Id { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
