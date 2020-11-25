using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Linq;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using AutoMapper;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IProductMappingService productMappingService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductMappingService productMappingService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.productMappingService = productMappingService;
        }

        public async Task AddNewAsync(ProductCreationDto productModel, IEnumerable<Photo> photos)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == productModel.CategoryId);
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == productModel.SupplierId);
            bool productAlreadyExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productModel.Id);

            if (productAlreadyExists)
            {
                throw new ProductAlreadyExistsException("A product with the given code already exists!");
            }
            if (categoryExists && supplierExists)
            {
                Category category = await unitOfWork.CategoryRepository.Get(productModel.CategoryId);
                Supplier supplier = await unitOfWork.SupplierRepository.Get(productModel.SupplierId);
                Product product = mapper.Map<ProductCreationDto, Product>(productModel);
                product.Category = category;
                await unitOfWork.ProductRepository.Add(product);

                Stock stock = new Stock
                {
                    NumberOfProducts = productModel.NumberOfProducts,
                    Product = product,
                    Supplier = supplier
                };
                await unitOfWork.StockRepository.Add(stock);

                foreach (var photo in photos)
                {
                    PhotoProduct photoProduct = new PhotoProduct
                    {
                        Photo = photo,
                        Product = product
                    };
                    await unitOfWork.PhotoProductRepository.Add(photoProduct);
                }
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The category or the supplier was not found...");
            }
        }

        public async Task<IEnumerable<string>> DeleteAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var photoNames = await unitOfWork.ProductRepository.Delete(id);
                await unitOfWork.SaveChangesAsync();
                return photoNames;
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllWithRelatedData();
            var mappedProducts = await productMappingService.MapProducts(products);
            return mappedProducts.OrderBy(x => x.CategoryName);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == categoryId);
            if (categoryExists)
            {
                var allProducts = await unitOfWork.ProductRepository.GetAllWithRelatedData();
                var searchedProducts = allProducts.Where(x => x.Category.Id == categoryId).ToList();

                return await productMappingService.MapProducts(searchedProducts);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsNoTracking(id);
                return await productMappingService.MapProductToProductDto(product);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<ProductEditingDto> GetForUpdateAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsNoTracking(id);
                return mapper.Map<Product, ProductEditingDto>(product);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<IEnumerable<string>> UpdateAsync(ProductEditingDto productDto, IEnumerable<Photo> photos)
        {
            IEnumerable<string> fileNames = new List<string>();
            var searchedProduct = await unitOfWork.ProductRepository.GetWithRelatedDataAsNoTracking(productDto.Id);
            var product = mapper.Map<ProductEditingDto, Product>(productDto);
            product.Category = searchedProduct.Category;

            unitOfWork.ProductRepository.Update(product);
            if (productDto.PhotoOption != PhotoOption.KeepCurrentPhotos)
            {
                if (photos.Count() > 0)
                {
                    if (productDto.PhotoOption == PhotoOption.DeleteCurrentPhotos)
                    {
                        fileNames = await unitOfWork.ProductRepository.GetPhotoNames(productDto.Id);
                        await unitOfWork.PhotoProductRepository.DeleteByProductId(productDto.Id);
                    }
                    foreach (var photo in photos)
                    {
                        PhotoProduct photoProduct = new PhotoProduct
                        {
                            Photo = photo,
                            Product = product
                        };
                        await unitOfWork.PhotoProductRepository.Add(photoProduct);
                    }
                }
            }
            await unitOfWork.SaveChangesAsync();
            return fileNames;
        }
    }
}
