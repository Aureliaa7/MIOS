using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task<List<string>> Register(RegistrationDto registrationInfo);
        Task<LoginResult> Login(LoginDto loginInfo);
        Task<AccountInfoDto> GetAccountInfo(Guid userId);
        Task Edit(Guid userId, AccountInfoDto accountInfo);
        Task<string> ChangePassword(ChangePasswordDto passwordDto);
    }
}
