using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class DeliveryMethodDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        [Range(-1, int.MaxValue, ErrorMessage = "Please enter a positive value")]
        public double Price { get; set; }
    }
}
