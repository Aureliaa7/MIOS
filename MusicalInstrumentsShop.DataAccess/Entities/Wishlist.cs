using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
    }
}
