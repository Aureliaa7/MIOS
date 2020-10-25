using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class WishlistProduct
    {
        public Guid Id { get; set; }
        public Wishlist Wishlist { get; set; }
        public Product Product { get; set; }
    }
}
