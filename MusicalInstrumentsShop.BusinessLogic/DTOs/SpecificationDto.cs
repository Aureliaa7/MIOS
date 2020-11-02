using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class SpecificationDto
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public string ProductId { get; set; }
    }
}
