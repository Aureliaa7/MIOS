using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.ProductFiltering
{
    public class ProductFilterService : IProductFilterService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IProductService productService;

        public ProductFilterService(IUnitOfWork unitOfWork, IProductService productService)
        {
            this.unitOfWork = unitOfWork;
            this.productService = productService;
        }

        public async Task<ProductsFilteringModel> Filter(ProductsFilteringModel data, int pageSize, int? pageNumber = 1)
        {
            var productsDto = await productService.GetAllAsync();
            if(data.CategoryId != null)
            {
                var category = await unitOfWork.CategoryRepository.Get((Guid)data.CategoryId);
                productsDto = productsDto.Where(x => x.CategoryName == category.Name).ToList();
            }

            if(data.OnlyProductsInStock == true)
            {
                foreach(var product in productsDto)
                {
                    var stock = await unitOfWork.StockRepository.GetByProductId(product.Id);
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
