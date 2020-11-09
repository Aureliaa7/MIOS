using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class ProductEditingDto
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        public IEnumerable<IFormFile> Photos { get; set; }
        [Required]
        [Display(Name = "Photo option")]
        public PhotoOption PhotoOption{ get; set; }
    }
}
