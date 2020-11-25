using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class ProductCreationDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [Required]
        [Display(Name = "Number of products")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int NumberOfProducts { get; set; }
        public Guid SupplierId { get; set; }
        [Required]
        public List<IFormFile> Photos { get; set; } 
    }
}
