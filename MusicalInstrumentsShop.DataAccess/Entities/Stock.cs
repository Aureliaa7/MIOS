using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Stock
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int NumberOfProducts { get; set; }
        public Supplier Supplier { get; set; }
    }
}