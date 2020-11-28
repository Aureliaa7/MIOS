using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class OrderDetails
    {
        public long Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Street { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DateTime OrderPlacementDate { get; set; }
        public ApplicationUser Customer { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        public double Amount { get; set; }
    }
}