using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public CreditCard CreditCard { get; set; }
        public DateTime Date { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
