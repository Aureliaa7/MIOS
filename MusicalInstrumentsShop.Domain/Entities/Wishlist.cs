using System;
using System.Collections.Generic;

namespace MusicalInstrumentsShop.Domain.Entities
{
    public class Wishlist
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
    }
}
