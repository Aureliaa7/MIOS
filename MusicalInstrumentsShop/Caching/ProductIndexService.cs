using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if(!memoryCache.TryGetValue<IEnumerable<ProductDto>>(key, out products))
            {
                products = await productService.GetAllAsync();
                memoryCache.Set<IEnumerable<ProductDto>>(key, products, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(2)
                });
            }
            else
            {
                products = (IEnumerable<ProductDto>)memoryCache.Get(key);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.CategoryName.Contains(searchString));
            }
            IEnumerable<ProductDto> orderedProducts = new List<ProductDto>();
            switch (orderCriteria)
            {
                case "name_desc":
                    orderedProducts = products.OrderByDescending(x => x.Name);
                    break;
                case "CategoryName":
                    orderedProducts = products.OrderBy(x => x.CategoryName);
                    break;
                case "category_desc":
                    orderedProducts = products.OrderByDescending(s => s.CategoryName);
                    break;
                case "SupplierName":
                    orderedProducts = products.OrderBy(x => x.SupplierName);
                    break;
                case "supplier_desc":
                    orderedProducts = products.OrderByDescending(s => s.SupplierName);
                    break;
                case "Id":
                    orderedProducts = products.OrderBy(x => x.Id);
                    break;
                case "id_desc":
                    orderedProducts = products.OrderByDescending(s => s.Id);
                    break;
                default:
                    orderedProducts = products.OrderBy(s => s.Name);
                    break;
            }
            return orderedProducts;
        }
    }
}
