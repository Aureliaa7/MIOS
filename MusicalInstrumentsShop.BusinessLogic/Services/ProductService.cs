using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Linq;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IStockRepository stockRepository;
        private readonly IPhotoProductRepository photoProductRepository;

        public ProductService(IProductRepository productRepository, IRepository<Category> categoryRepository,
            IRepository<Supplier> supplierRepository, IStockRepository stockRepository, IPhotoProductRepository photoProductRepository)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.supplierRepository = supplierRepository;
            this.stockRepository = stockRepository;
            this.photoProductRepository = photoProductRepository;
        }

        public async Task AddNew(ProductCreationDto productModel, IEnumerable<Photo> photos)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == productModel.CategoryId);
            bool supplierExists = await supplierRepository.Exists(x => x.Id == productModel.SupplierId);
            bool productAlreadyExists = await productRepository.Exists(x => x.Id == productModel.Id);

            if (productAlreadyExists)
            {
                throw new ProductAlreadyExistsException("A product with the given code already exists!");
            }
            if (categoryExists && supplierExists)
            {
                Category category = await categoryRepository.Get(productModel.CategoryId);
                Supplier supplier = await supplierRepository.Get(productModel.SupplierId);
                Product product = new Product
                {
                    Category = category,
                    Id = productModel.Id,
                    Description = productModel.Description,
                    Name = productModel.Name,
                    Price = productModel.Price
                };
                await productRepository.Add(product);

                Stock stock = new Stock
                {
                    NumberOfProducts = productModel.NumberOfProducts,
                    Product = product,
                    Supplier = supplier
                };
                await stockRepository.Add(stock);

                foreach (var photo in photos)
                {
                    PhotoProduct photoProduct = new PhotoProduct
                    {
                        Photo = photo,
                        Product = product
                    };
                    await photoProductRepository.Add(photoProduct);
                }
            }
            else
            {
                throw new ItemNotFoundException("The category or the supplier was not found...");
            }
        }

        public async Task<IEnumerable<string>> Delete(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                return await productRepository.Delete(id);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            var products = await productRepository.GetAllWithRelatedData();
            return await MapProducts(products);
        }

        public async Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == categoryId);
            if (categoryExists)
            {
                var allProducts = await productRepository.GetAllWithRelatedData();
                var searchedProducts = allProducts.Where(x => x.Category.Id == categoryId).ToList();

                return await MapProducts(searchedProducts);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task<ProductDto> GetById(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await productRepository.GetWithRelatedData(id);
                return await MapProductToProductDto(product);
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<ProductEditingDto> GetForUpdate(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await productRepository.GetWithRelatedData(id);
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

        public async Task<IEnumerable<string>> Update(ProductEditingDto productDto, IEnumerable<Photo> photos)
        {
            IEnumerable<string> fileNames = new List<string>();
            var product = await productRepository.GetWithRelatedData(productDto.Id);
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = product.Description;
            await productRepository.Update(product);
            if (productDto.PhotoOption != PhotoOption.KeepCurrentPhotos)
            {
                if (photos != null)
                {
                    if (productDto.PhotoOption == PhotoOption.DeleteCurrentPhotos)
                    {
                        fileNames = await productRepository.GetPhotoNames(productDto.Id);
                        await photoProductRepository.DeleteByProductId(productDto.Id);
                    }
                    foreach (var photo in photos)
                    {
                        PhotoProduct photoProduct = new PhotoProduct
                        {
                            Photo = photo,
                            Product = product
                        };
                        await photoProductRepository.Add(photoProduct);
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
            Stock stock = await stockRepository.GetByProductId(product.Id);
            ProductDto productDto = null;
            if (stock != null)
            {
                var photos = await photoProductRepository.GetByProductId(product.Id);
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
