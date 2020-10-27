using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task<List<string>> Register(RegistrationDto registrationInfo);
        Task<LoginResult> Login(LoginDto loginInfo);
    }
}
