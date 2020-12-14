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
    public class CartProductService : ICartProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProductMappingService productMappingService;
        private readonly IStockService stockService;

        public CartProductService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, 
            IProductMappingService productMappingService, IStockService stockService)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.productMappingService = productMappingService;
            this.stockService = stockService;
        }

        public async Task AddAsync(CartProductCreationDto cartProductCreationDto)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == cartProductCreationDto.ProductId);
            var user = await userManager.FindByIdAsync(cartProductCreationDto.UserId.ToString());
            if (productExists && user != null)
            {
                var cartProducts = await unitOfWork.CartProductRepository.GetByUserId(cartProductCreationDto.UserId);
                bool productIsInCart = cartProducts.Where(x => x.Product.Id == cartProductCreationDto.ProductId).Any();
                if (!productIsInCart)
                {
                    var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsTracking(cartProductCreationDto.ProductId);
                    var cart = await unitOfWork.CartRepository.GetByUserId(cartProductCreationDto.UserId);
                    if (cart != null)
                    {
                        var cartProduct = new CartProduct
                        {
                            Product = product,
                            Cart = cart,
                            NumberOfProducts = cartProductCreationDto.NumberOfProducts
                        };
                        await unitOfWork.CartProductRepository.Add(cartProduct);
                    }
                    else
                    {
                        throw new ItemNotFoundException("The cart was not found...");
                    }
                }
                else
                {
                    var cartProduct = await unitOfWork.CartProductRepository.GetByProduct(cartProductCreationDto.ProductId);
                    cartProduct.NumberOfProducts++;
                    unitOfWork.CartProductRepository.Update(cartProduct);
                }
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The product or the user was not found...");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            bool cartProductExists = await unitOfWork.CartProductRepository.Exists(x => x.Id == id);
            if (cartProductExists)
            {
                await unitOfWork.CartProductRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The item was not found...");
            }
        }

        public async Task EmptyCart(Guid cartId)
        {
            bool cartExists = await unitOfWork.CartRepository.Exists(x => x.Id == cartId);
            if (cartExists)
            {
                var cartProducts = await unitOfWork.CartProductRepository.GetByCartId(cartId);
                if (cartProducts != null)
                {
                    unitOfWork.CartProductRepository.RemoveRange(cartProducts);
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<CartProductDto>> GetAllAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var cartProducts = await unitOfWork.CartProductRepository.GetByUserId(userId);
                var cartProductsDto = new List<CartProductDto>();
                foreach (var cartProduct in cartProducts)
                {
                    var productDto = await productMappingService.MapProductToProductDto(cartProduct.Product);
                    var cartProductDto = new CartProductDto
                    {
                        Id = cartProduct.Id,
                        Product = productDto,
                        CartId = cartProduct.Cart.Id,
                        NumberOfProducts = cartProduct.NumberOfProducts
                    };
                    cartProductsDto.Add(cartProductDto);
                }
                return cartProductsDto;
            }
            throw new ItemNotFoundException("The user was not found...");
        }

        public async Task<bool> UpdateNumberOfProductsAsync(string productCode, int numberOfProducts)
        {
            bool cartProductExists = await unitOfWork.CartProductRepository.Exists(x => x.Product.Id == productCode);
            if (cartProductExists)
            {
                var cartProduct = await unitOfWork.CartProductRepository.GetByProduct(productCode);
                int quantityDifference = Math.Abs(cartProduct.NumberOfProducts - numberOfProducts);
                if (numberOfProducts > 0 && await stockService.CanTakeAsync(quantityDifference, cartProduct.Product.Id))
                {
                    cartProduct.NumberOfProducts = numberOfProducts;
                    unitOfWork.CartProductRepository.Update(cartProduct);
                    await unitOfWork.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
