using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class AddProductModel
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
        public int NumberOfProducts { get; set; }
        public Guid SupplierId { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
