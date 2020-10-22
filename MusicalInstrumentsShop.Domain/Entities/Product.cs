using System;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public string Photo { get; set; }
    }
}
