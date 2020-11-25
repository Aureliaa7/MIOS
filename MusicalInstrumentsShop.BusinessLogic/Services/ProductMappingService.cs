using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductMappingService : IProductMappingService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductMappingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> MapProducts(IEnumerable<Product> products)
        {
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var productDto = await MapProductToProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        public async Task<ProductDto> MapProductToProductDto(Product product)
        {
            Stock stock = await unitOfWork.StockRepository.GetByProductId(product.Id);
            ProductDto productDto = null;
            if (stock != null)
            {
                var specifications = await unitOfWork.SpecificationRepository.GetByProductId(product.Id);
                var specificationDtos = new List<SpecificationDto>();
                if (specifications != null)
                {
                    foreach (var specification in specifications)
                    {
                        var specificationDto = mapper.Map<Specification, SpecificationDto>(specification);
                        specificationDtos.Add(specificationDto);
                    }
                }
                var photos = await unitOfWork.PhotoProductRepository.GetByProductId(product.Id);
                productDto = mapper.Map<Product, ProductDto>(product);
                productDto.CategoryName = product.Category.Name;
                productDto.SupplierName = stock.Supplier.Name;
                productDto.Photos = photos;
                productDto.NumberOfProducts = stock.NumberOfProducts;
                productDto.Specifications = specificationDtos;
            }
            return productDto;
        }
    }
}
