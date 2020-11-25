using System;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class WishlistProductCreationDto
    {
        public Guid UserId { get; set; }
        public string ProductId { get; set; }
    }
}
