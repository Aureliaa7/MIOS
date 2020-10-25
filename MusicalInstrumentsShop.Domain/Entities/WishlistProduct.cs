using System;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class WishlistProduct
    {
        public Guid Id { get; set; }
        public Wishlist Wishlist { get; set; }
        public Product Product { get; set; }
    }
}
