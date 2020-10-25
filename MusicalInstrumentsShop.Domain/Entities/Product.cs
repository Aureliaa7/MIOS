using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Product
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
        public Category Category { get; set; }
    }
}
