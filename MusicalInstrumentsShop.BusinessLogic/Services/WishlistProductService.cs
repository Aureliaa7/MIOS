using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class WishlistProductService : IWishlistProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductMappingService productMappingService;

        public WishlistProductService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IProductMappingService productMappingService)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.productMappingService = productMappingService;
        }

        public async Task AddAsync(WishlistProductCreationDto wishlistCreationDto)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == wishlistCreationDto.ProductId);
            var user = await userManager.FindByIdAsync(wishlistCreationDto.UserId.ToString());
            if (productExists && user != null)
            {
                var wishlistProducts = await unitOfWork.WishlistProductRepository.GetByUserId(wishlistCreationDto.UserId);
                bool productIsInWishlist = wishlistProducts.Where(x => x.Product.Id == wishlistCreationDto.ProductId).Any();
                if (!productIsInWishlist)
                {
                    var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsTracking(wishlistCreationDto.ProductId);
                    var wishlist = await unitOfWork.WishlistRepository.GetByUserId(wishlistCreationDto.UserId);
                    if (wishlist != null)
                    {
                        var wishlistProduct = new WishlistProduct
                        {
                            Product = product,
                            Wishlist = wishlist
                        };
                        await unitOfWork.WishlistProductRepository.Add(wishlistProduct);
                        await unitOfWork.SaveChangesAsync();
                    }
                }
            }
            else
            {
                throw new ItemNotFoundException("The product or the user was not found...");
            } 
        }

        public async Task DeleteAsync(Guid id)
        {
            bool wishlistProductExists = await unitOfWork.WishlistProductRepository.Exists(x => x.Id == id);
            if(wishlistProductExists)
            {
                await unitOfWork.WishlistProductRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The item was not found...");
            } 
        }

        public async Task<IEnumerable<WishlistProductDto>> GetAllAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if(user != null)
            {
                var wishlistProducts = await unitOfWork.WishlistProductRepository.GetByUserId(userId);
                var wishlistProductsDto = new List<WishlistProductDto>();
                foreach(var wishlistProduct in wishlistProducts)
                {
                    var productDto = await productMappingService.MapProductToProductDto(wishlistProduct.Product);
                    var wishlistProductDto = new WishlistProductDto { Id = wishlistProduct.Id, Product = productDto };
                    wishlistProductsDto.Add(wishlistProductDto);
                }
                return wishlistProductsDto;
            }
            throw new ItemNotFoundException("The user was not found...");
        }
    }
}
