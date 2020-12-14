using AutoMapper;
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
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IStockService stockService;
        private readonly ICartProductService cartProductService;

        public OrderDetailsService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager,
            ICartProductService cartProductService, IStockService stockService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
            this.stockService = stockService;
            this.cartProductService = cartProductService;
        }

        public async Task AddAsync(OrderDetailsDto orderDetailsDto)
        {
            bool deliveryMethodExists = await unitOfWork.DeliveryMethodRepository.Exists(x => x.Id == orderDetailsDto.DeliveryMethodId);
            bool paymentMethodExits = await unitOfWork.PaymentMethodRepository.Exists(x => x.Id == orderDetailsDto.PaymentMethodId);
            var customer = await userManager.FindByIdAsync(orderDetailsDto.CustomerId.ToString());
            if (deliveryMethodExists && paymentMethodExits && customer != null)
            {
                var orderDetails = mapper.Map<OrderDetails>(orderDetailsDto);
                var deliveryMethod = await unitOfWork.DeliveryMethodRepository.Get(orderDetailsDto.DeliveryMethodId);
                var paymentMethod = await unitOfWork.PaymentMethodRepository.Get(orderDetailsDto.PaymentMethodId);
                orderDetails.Customer = customer;
                orderDetails.DeliveryMethod = deliveryMethod;
                orderDetails.PaymentMethod = paymentMethod;
                orderDetails.Status = OrderStatus.InProgress;
                orderDetails.OrderPlacementDate = DateTime.Now;
                var cartProducts = await cartProductService.GetAllAsync(orderDetailsDto.CustomerId);
                orderDetails.Amount = cartProducts.Sum(x => x.Product.Price * x.NumberOfProducts) + deliveryMethod.Price;
                await unitOfWork.OrderDetailsRepository.Add(orderDetails);
                if (cartProducts != null)
                {
                    var cartId = cartProducts.ElementAt(0).CartId;
                    foreach (var item in cartProducts)
                    {
                        var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsTracking(item.Product.Id);
                        var orderProduct = new OrderProduct
                        {
                            Id = Guid.NewGuid(),
                            NumberOfProducts = item.NumberOfProducts,
                            OrderDetails = orderDetails,
                            Product = product
                        };
                        await stockService.DecreaseNumberOfProductsAsync(item.NumberOfProducts, item.Product.Id);
                        await unitOfWork.OrderProductRepository.Add(orderProduct);
                    }
                    await cartProductService.EmptyCart(cartId);
                    await unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                throw new ItemNotFoundException("Item not found...");
            }
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetAllAsync(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var orderDetails = await unitOfWork.OrderDetailsRepository.GetAllWithRelatedData(userId);
                var orderDetailsDtos = await MapOrders(orderDetails);
                return orderDetailsDtos;
            }
            throw new ItemNotFoundException("The user was not found...");
        }

        public async Task<OrderDetailsDto> GetByIdAsync(long id)
        {
            bool orderDetailsExists = await unitOfWork.OrderDetailsRepository.Exists(x => x.Id == id);
            if (orderDetailsExists)
            {
                var orderDetails = await unitOfWork.OrderDetailsRepository.GetByIdWithRelatedData(id);
                var orderDetailsDto = mapper.Map<OrderDetailsDto>(orderDetails);
                var orderProducts = await unitOfWork.OrderProductRepository.GetByOrderDetailsId(id);
                var items = new List<CartProductDto>();
                foreach (var orderProduct in orderProducts)
                {
                    items.Add(new CartProductDto
                    {
                        Product = mapper.Map<ProductDto>(orderProduct.Product),
                        NumberOfProducts = orderProduct.NumberOfProducts
                    });
                }
                var deliveryMethod = await unitOfWork.DeliveryMethodRepository.Get(orderDetails.DeliveryMethod.Id);
                orderDetailsDto.PaymentMethodName = orderDetails.PaymentMethod.Name;
                orderDetailsDto.DeliveryMethodName = orderDetails.DeliveryMethod.Method;
                orderDetailsDto.CartProducts = items;
                orderDetailsDto.DeliveryPrice = deliveryMethod.Price;
                return orderDetailsDto;
            }
            throw new ItemNotFoundException("The order was not found...");
        }

        public async Task<IEnumerable<OrderDetailsDto>> GetByStatusAsync(Guid userId, OrderStatus status)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var orderDetails = await unitOfWork.OrderDetailsRepository.GetAllWithRelatedData(userId);
                orderDetails = orderDetails.Where(x => x.Status == status).ToList();
                var orderDetailsDtos = await MapOrders(orderDetails);
                return orderDetailsDtos;
            }
            throw new ItemNotFoundException("The user was not found...");
        }

        public async Task UpdateStatusAsync(long orderDetailsId, OrderStatus status)
        {
            bool orderDetailsExists = await unitOfWork.OrderDetailsRepository.Exists(x => x.Id == orderDetailsId);
            if (orderDetailsExists)
            {
                var orderDetails = await unitOfWork.OrderDetailsRepository.GetByIdWithRelatedData(orderDetailsId);
                orderDetails.Status = status;
                unitOfWork.OrderDetailsRepository.Update(orderDetails);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The order was not found...");
            }
        }

        private async Task<IEnumerable<OrderDetailsDto>> MapOrders(IEnumerable<OrderDetails> orderDetails)
        {
            var orderDetailsDtos = new List<OrderDetailsDto>();
            foreach (var orderDetail in orderDetails)
            {
                var orderDetailsDto = mapper.Map<OrderDetailsDto>(orderDetail);
                var orderProducts = await unitOfWork.OrderProductRepository.GetByOrderDetailsId(orderDetail.Id);
                var items = new List<CartProductDto>();
                foreach (var orderProduct in orderProducts)
                {
                    var item = new CartProductDto
                    {
                        Product = mapper.Map<ProductDto>(orderProduct.Product),
                        NumberOfProducts = orderProduct.NumberOfProducts
                    };
                    items.Add(item);
                }
                var delivery = await unitOfWork.DeliveryMethodRepository.Get(orderDetail.DeliveryMethod.Id);
                var payment = await unitOfWork.PaymentMethodRepository.Get(orderDetail.PaymentMethod.Id);
                orderDetailsDto.CartProducts = items;
                orderDetailsDto.DeliveryMethodName = delivery.Method;
                orderDetailsDto.PaymentMethodName = payment.Name;
                orderDetailsDto.DeliveryPrice = delivery.Price;
                orderDetailsDto.Amount = orderDetail.Amount;
                orderDetailsDtos.Add(orderDetailsDto);
            }
            return orderDetailsDtos;
        }
    }
}
