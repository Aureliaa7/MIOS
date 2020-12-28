using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities
{
    public class ProductsFilteringModel
    {
        public PaginatedList<ProductDto> Products { get; set; }
        public bool OnlyProductsInStock { get; set; }
        public Guid? CategoryId { get; set; }
        [Range(1, 100000000, ErrorMessage = "The price should be greater than 0")]
        public float? MinPrice { get; set; }
        [Range(1, 100000000, ErrorMessage = "The price should be greater than 0")]
        public float? MaxPrice { get; set; }
    }
}
