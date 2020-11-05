using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Linq;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
                Product product = new Product
                {
                    Category = category,
                    Id = productModel.Id,
                    Description = productModel.Description,
                    Name = productModel.Name,
                    Price = productModel.Price
                };
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
                return await unitOfWork.ProductRepository.Delete(id);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllWithRelatedData();
            return await MapProducts(products);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == categoryId);
            if (categoryExists)
            {
                var allProducts = await unitOfWork.ProductRepository.GetAllWithRelatedData();
                var searchedProducts = allProducts.Where(x => x.Category.Id == categoryId).ToList();

                return await MapProducts(searchedProducts);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task<ProductDto> GetByIdAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedData(id);
                return await MapProductToProductDto(product);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<ProductEditingDto> GetForUpdateAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedData(id);
                var updateProductDto = new ProductEditingDto
                {
                    Id = product.Id,
                    Description = product.Description,
                    Name = product.Name,
                    Price = product.Price
                };
                return updateProductDto;
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<IEnumerable<string>> UpdateAsync(ProductEditingDto productDto, IEnumerable<Photo> photos)
        {
            IEnumerable<string> fileNames = new List<string>();
            var product = await unitOfWork.ProductRepository.GetWithRelatedData(productDto.Id);
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = product.Description;
            await unitOfWork.ProductRepository.Update(product);
            if (productDto.PhotoOption != PhotoOption.KeepCurrentPhotos)
            {
                if (photos != null)
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
            return fileNames;
        }

        private async Task<IEnumerable<ProductDto>> MapProducts(IEnumerable<Product> products)
        {
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                var productDto = await MapProductToProductDto(product);
                productDtos.Add(productDto);
            }
            return productDtos;
        }

        private async Task<ProductDto> MapProductToProductDto(Product product)
        {
            Stock stock = await unitOfWork.StockRepository.GetByProductId(product.Id);
            ProductDto productDto = null;
            if (stock != null)
            {
                var photos = await unitOfWork.PhotoProductRepository.GetByProductId(product.Id);
                productDto = new ProductDto
                {
                    CategoryName = product.Category.Name,
                    Description = product.Description,
                    Id = product.Id,
                    Name = product.Name,
                    SupplierName = stock.Supplier.Name,
                    Photos = photos,
                    Price = product.Price,
                    NumberOfProducts = stock.NumberOfProducts
                };
            }
            return productDto;
        }
    }
}
