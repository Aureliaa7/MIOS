using System;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class WishlistProductDto
    {
        public Guid Id { get; set; }
        public ProductDto Product { get; set; }
    }
}
