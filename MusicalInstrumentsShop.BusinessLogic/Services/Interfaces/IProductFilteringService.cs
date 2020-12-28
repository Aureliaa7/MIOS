using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IProductFilteringService
    {
        Task<ProductsFilteringModel> Filter(IEnumerable<ProductDto> productsDto, ProductsFilteringModel data, int pageSize, int? pageNumber = 1);
    }
}
