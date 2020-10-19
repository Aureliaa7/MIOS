using MusicalInstrumentsShop.BusinessLogic.HelperEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task<List<string>> Register(RegistrationModel registrationInfo);
        Task<LoginResult> Login(LoginModel loginInfo);
    }
}
