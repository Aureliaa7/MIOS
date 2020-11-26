using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class OrderDetailsDto
    {
        public long Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string Telephone { get; set; }
        [Display(Name = "Delivery")]
        public string DeliveryMethodName { get; set; }
        [Required]
        public Guid DeliveryMethodId { get; set; }
        public DateTime OrderPlacementDate { get; set; }
        public Guid CustomerId { get; set; }
        [Display(Name = "Payment")]
        public string PaymentMethodName { get; set; }
        [Required]
        public Guid PaymentMethodId { get; set; }
        public OrderStatus Status { get; set; }
        public List<Item> Items { get; set; }
    }
}
