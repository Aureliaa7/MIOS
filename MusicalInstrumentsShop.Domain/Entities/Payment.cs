using System;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Order Order { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardHoldersName { get; set; }
        public DateTime CardExpirationDate { get; set; }
    }
}
