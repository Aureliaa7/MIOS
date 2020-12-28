using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public class ProductIndexService : IProductIndexService
    {
        private readonly IProductService productService;
        private string key;

        public ProductIndexService(IProductService productService, IConfiguration configuration)
        {
            this.productService = productService;
            key = configuration.GetValue<string>("ProductKeys:IndexProducts");
        }

        public async Task<IEnumerable<ProductDto>> OrderByCriteria(string searchString, string orderCriteria, IMemoryCache memoryCache)
        {
            IEnumerable<ProductDto> products = new List<ProductDto>();
            if(!memoryCache.TryGetValue(key, out products))
            {
                products = await productService.GetAllAsync();
                memoryCache.Set(key, products);
            }
            else
            {
                products = (IEnumerable<ProductDto>)memoryCache.Get(key);
            }

            return await productService.Order(products, searchString, orderCriteria);
        }
    }
}
