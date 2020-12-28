using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public class ProductBrowsingService : IProductBrowsingService
    {
        private readonly IProductService productService;
        private readonly IProductFilteringService productFilteringService;
        private string key;

        public ProductBrowsingService(IProductService productService, 
            IProductFilteringService productFilteringService, IConfiguration configuration)
        {
            this.productService = productService;
            this.productFilteringService = productFilteringService;
            key = configuration.GetValue<string>("ProductKeys:BrowseProducts");
        }

        public async Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, IMemoryCache memoryCache, int pageSize, int? pageNumber = 1)
        {
            IEnumerable<ProductDto> productsDto = new List<ProductDto>();
            if (!memoryCache.TryGetValue(key, out productsDto))
            {
                productsDto = await productService.GetAllAsync();
                memoryCache.Set(key, productsDto);
            }
            else
            {
                productsDto = (IEnumerable<ProductDto>)memoryCache.Get(key);
            }

            return await productFilteringService.Filter(productsDto, data, pageSize, pageNumber);
        }
    }
}
