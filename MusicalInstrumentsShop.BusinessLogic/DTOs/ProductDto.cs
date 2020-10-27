using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class ProductDto
    {
        [Display(Name = "Code")]
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        [Display(Name = "Category")]
        public string CategoryName { get; set; }
        [Display(Name = "Number of products")]
        public int NumberOfProducts { get; set; }
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
