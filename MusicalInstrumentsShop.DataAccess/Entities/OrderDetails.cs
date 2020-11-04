using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class OrderDetails
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DateTime OrderPlacementDate { get; set; }
        public ApplicationUser Customer { get; set; }
    }
}