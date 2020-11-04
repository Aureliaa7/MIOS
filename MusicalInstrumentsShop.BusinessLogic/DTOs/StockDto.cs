using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class StockDto
    {
        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public Guid SupplierId { get; set; }
        [Required]
        [Display(Name = "Product")]
        public string ProductId { get;set;}
        [Required]
        [Display(Name = "Number of products")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int NumberOfProducts { get; set; }
    }
}
