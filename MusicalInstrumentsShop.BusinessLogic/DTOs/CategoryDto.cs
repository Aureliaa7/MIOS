using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(pattern: "[a-zA-Z\\s]+", ErrorMessage = "Invalid text")]
        public string Name { get; set; }
    }
}
