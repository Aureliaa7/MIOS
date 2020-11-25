using Microsoft.AspNetCore.Identity;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public WishlistService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task CreateAsync(string userEmail)
        {
            var user = await userManager.FindByNameAsync(userEmail);
            if(user != null)
            {
                var wishlist = new Wishlist
                {
                    Id = Guid.NewGuid(),
                    User = user
                };
                await unitOfWork.WishlistRepository.Add(wishlist);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The user was not found...");
            }
        }
    }
}
