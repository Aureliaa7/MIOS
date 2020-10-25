using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public CreditCard CreditCard { get; set; }
        public Order Order { get; set; }
    }
}
