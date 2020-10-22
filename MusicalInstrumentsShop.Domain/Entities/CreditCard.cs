using System;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class CreditCard
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string HolderName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public ApplicationUser User { get; set; }
    }
}
