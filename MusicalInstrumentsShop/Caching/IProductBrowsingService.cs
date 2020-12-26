using Microsoft.Extensions.Caching.Memory;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public interface IProductBrowsingService
    {
        public Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, IMemoryCache memoryCache, int pageSize, int? pageNumber = 1);
    }
}