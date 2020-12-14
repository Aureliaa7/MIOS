using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface ICartService
    {
        Task CreateAsync(string userEmail);
    }
}
