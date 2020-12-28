using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.ProductFilteringEntities;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductFilteringService : IProductFilteringService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductFilteringService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ProductsFilteringModel> Filter(IEnumerable<ProductDto>productsDto, ProductsFilteringModel data, int pageSize, int? pageNumber = 1)
        {
            if (data.CategoryId != null)
            {
                var category = await unitOfWork.CategoryRepository.Get((Guid)data.CategoryId);
                productsDto = productsDto.Where(x => x.CategoryName == category.Name).ToList();
            }

            if (data.OnlyProductsInStock == true)
            {
                foreach (var product in productsDto)
                {
                    var stock = await unitOfWork.StockRepository.GetByProductId(product.Id);
                    if (stock.NumberOfProducts == 0)
                    {
                        productsDto = productsDto.Where(x => x.Id != product.Id);
                    }
                }
            }

            if (data.MinPrice != null && data.MaxPrice != null)
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
