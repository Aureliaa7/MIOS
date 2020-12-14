using System;

namespace MusicalInstrumentsShop.BusinessLogic.DTOs
{
    public class CartProductCreationDto
    {
        public Guid UserId { get; set; }
        public string ProductId { get; set; }
        public int NumberOfProducts { get; set; }
    }
}
