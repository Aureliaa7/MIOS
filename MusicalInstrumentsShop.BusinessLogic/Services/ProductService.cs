using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;

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

        public async Task AddNew(AddProductDto productModel, IEnumerable<Photo> photos)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == productModel.CategoryId);
            bool supplierExists = await supplierRepository.Exists(x => x.Id == productModel.SupplierId);

            if(categoryExists && supplierExists)
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

                foreach(var photo in photos)
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
            var productDtos = new List<ProductDto>();
            var products = await productRepository.GetAllWithRelatedData();
            foreach(var product in products)
            {
                Stock stock = await stockRepository.GetByProductId(product.Id);
                if(stock != null)
                {
                    var photos = await photoProductRepository.GetByProductId(product.Id);
                    var productDto = new ProductDto
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
                    productDtos.Add(productDto);       
                }
            }
            return productDtos;
        }

        public async Task<ProductDto> GetById(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var product = await productRepository.GetWithRelatedData(id);
                var stock = await stockRepository.GetByProductId(product.Id);
                if(stock != null)
                {
                    var photos = await photoProductRepository.GetByProductId(product.Id);
                    return new ProductDto
                    {
                        CategoryName = product.Category.Name,
                        Description = product.Description,
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Photos = photos,
                        NumberOfProducts = stock.NumberOfProducts,
                        SupplierName = stock.Supplier.Name
                    };
                }
            }
            throw new ItemNotFoundException("The product was not found...");
        }

        public async Task<UpdateProductDto> GetForUpdate(string id)
        {
            bool productExists = await productRepository.Exists(x => x.Id == id);
            if(productExists)
            {
                var product = await productRepository.GetWithRelatedData(id);
                var updateProductDto = new UpdateProductDto
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

        public async Task Update(UpdateProductDto productDto, IEnumerable<Photo> photos)
        {
            var product = await productRepository.GetWithRelatedData(productDto.Id);
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = product.Description;
            await productRepository.Update(product);
            await photoProductRepository.DeleteByProductId(productDto.Id);

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
}
