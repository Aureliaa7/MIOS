using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Shipping
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public DateTime Date { get; set; }
        public Order Order { get; set; }
    }
}