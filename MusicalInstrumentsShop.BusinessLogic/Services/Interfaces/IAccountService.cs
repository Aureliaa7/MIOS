using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<string>> RegisterAsync(RegistrationDto registrationInfo);
        Task<LoginResult> LoginAsync(LoginDto loginInfo);
        Task<AccountInfoDto> GetAccountInfoAsync(Guid userId);
        Task EditAsync(Guid userId, AccountInfoDto accountInfo);
        Task<string> ChangePasswordAsync(PasswordChangeDto passwordDto);
    }
}
