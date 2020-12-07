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
        private readonly IImageService imageService;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IProductMappingService productMappingService, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
            this.productMappingService = productMappingService;
        }

        public async Task AddNewAsync(ProductCreationDto productModel)
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
                IEnumerable<Photo> photos = new List<Photo>();
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
                photos = imageService.SaveFiles(productModel.Photos);
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

        public async Task DeleteAsync(string id)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == id);
            if (productExists)
            {
                var photoNames = await unitOfWork.ProductRepository.Delete(id);
                await imageService.DeleteFiles(photoNames);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The product was not found...");
            }
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

        public async Task<int> GetNoProductsAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAll();
            return products.Count();
        }

        public async Task<IEnumerable<ProductDto>> Order(string searchString, string orderCriteria)
        {
            var products = await GetAllAsync();

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

        public async Task UpdateAsync(ProductEditingDto productDto)
        {
            IEnumerable<string> fileNames = new List<string>();
            IEnumerable<Photo> photos = new List<Photo>();
            var searchedProduct = await unitOfWork.ProductRepository.GetWithRelatedDataAsNoTracking(productDto.Id);
            var product = mapper.Map<ProductEditingDto, Product>(productDto);
            product.Category = searchedProduct.Category;

            unitOfWork.ProductRepository.Update(product);
            if (productDto.PhotoOption != PhotoOption.KeepCurrentPhotos && productDto.Photos != null)
            {
                photos = imageService.SaveFiles(productDto.Photos);

                if (photos.Count() > 0)
                {
                    if (productDto.PhotoOption == PhotoOption.DeleteCurrentPhotos)
                    {
                        fileNames = await unitOfWork.ProductRepository.GetPhotoNames(productDto.Id);
                        await unitOfWork.PhotoProductRepository.DeleteByProductId(productDto.Id);
                        await imageService.DeleteFiles(fileNames);
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
        }
    }
}
