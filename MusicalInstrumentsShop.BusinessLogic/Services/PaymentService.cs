using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<PaymentMethodDto>> GetAllAsync()
        {
            var paymentMethods = await unitOfWork.PaymentMethodRepository.GetAll();
            var paymentMethodsDtos = new List<PaymentMethodDto>();
            foreach(var method in paymentMethods)
            {
                paymentMethodsDtos.Add(mapper.Map<PaymentMethodDto>(method));
            }
            return paymentMethodsDtos;
        }
    }
}
