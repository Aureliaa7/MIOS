using System;

namespace MusicalInstrumentsShop.DataAccess.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public ApplicationUser Customer { get; set; }
    }
}
