using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentMethodDto>> GetAllAsync();
    }
}
