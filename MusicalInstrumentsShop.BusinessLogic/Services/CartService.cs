using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public CartService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task CreateAsync(string userEmail)
        {
            var customer = await userManager.FindByNameAsync(userEmail);
            if (customer != null)
            {
                var cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    Customer = customer
                };
                await unitOfWork.CartRepository.Add(cart);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The user was not found...");
            }
        }
    }
}
