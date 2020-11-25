using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IProductMappingService
    {
        Task<IEnumerable<ProductDto>> MapProducts(IEnumerable<Product> products);
        Task<ProductDto> MapProductToProductDto(Product product);
    }
}
