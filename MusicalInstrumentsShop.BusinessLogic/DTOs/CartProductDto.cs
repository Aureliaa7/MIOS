using System;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class CartProductDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public ProductDto Product { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
