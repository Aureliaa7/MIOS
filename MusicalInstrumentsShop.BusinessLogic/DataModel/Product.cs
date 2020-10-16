using System;

namespace MusicalInstrumentsShop.BusinessLogic.DataModel
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Specifications { get; set; }
        public Stock Stock { get; set; }
        public Category Category { get; set; }
        // add the photo


    }
}
