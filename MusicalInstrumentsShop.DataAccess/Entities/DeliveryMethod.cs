using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class DeliveryMethod
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public double Price { get; set; }
    }
}