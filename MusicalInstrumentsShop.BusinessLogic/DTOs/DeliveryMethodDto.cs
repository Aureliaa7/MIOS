using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class DeliveryMethodDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Method { get; set; }
    }
}
