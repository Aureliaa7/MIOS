using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IWishlistService
    {
        Task CreateAsync(string userEmail);
    }
}
