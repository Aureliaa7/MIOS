using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Category
    { 
        public Guid Id { get; set; }
        [Required]
        [RegularExpression(pattern: "[a-zA-Z\\s]+", ErrorMessage = "Invalid text")]
        public string Name { get; set; }
    }
}