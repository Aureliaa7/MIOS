using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.ProductFiltering;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.Caching
{
    public class ProductBrowsingService : IProductBrowsingService
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        private readonly IStockService stockService;
        private string key;

        public ProductBrowsingService(IProductService productService, ICategoryService categoryService, 
            IStockService stockService, IConfiguration configuration)
        {
            this.productService = productService;
            this.categoryService = categoryService;
            this.stockService = stockService;
            key = configuration.GetValue<string>("ProductKeys:BrowseProducts");
        }

        public async Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, IMemoryCache memoryCache, int pageSize, int? pageNumber = 1)
        {
            IEnumerable<ProductDto> productsDto = new List<ProductDto>();
            if (!memoryCache.TryGetValue<IEnumerable<ProductDto>>(key, out productsDto))
            {
                productsDto = await productService.GetAllAsync();
                memoryCache.Set<IEnumerable<ProductDto>>(key, productsDto, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(2)
                });
            }
            else
            {
                productsDto = (IEnumerable<ProductDto>)memoryCache.Get(key);
            }
            if(data.CategoryId != null)
            {
                var category = await categoryService.GetByIdAsync((Guid)data.CategoryId);
                productsDto = productsDto.Where(x => x.CategoryName == category.Name).ToList();
            }

            if(data.OnlyProductsInStock == true)
            {
                foreach(var product in productsDto)
                {
                    var stock = await stockService.GetByProductAsync(product.Id);
                    if(stock.NumberOfProducts == 0)
                    {
                        productsDto = productsDto.Where(x => x.Id != product.Id);
                    }
                }
            }

            if(data.MinPrice != null && data.MaxPrice != null)
            {
                productsDto = productsDto.Where(x => x.Price >= data.MinPrice && x.Price <= data.MaxPrice);
            }

            return new ProductsFilteringModel
            {
                Products = PaginatedList<ProductDto>.Create(productsDto, pageNumber ?? 1, pageSize)
            };
        }
    }
}
