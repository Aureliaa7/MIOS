using Microsoft.Extensions.Caching.Memory;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public interface IProductBrowsingService
    {
        Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, IMemoryCache memoryCache, int pageSize, int? pageNumber = 1);
    }
}