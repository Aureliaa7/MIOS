using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public DeliveryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddAsync(DeliveryMethodDto deliveryMethod)
        {
            DeliveryMethod delivery = mapper.Map<DeliveryMethod>(deliveryMethod);
            await unitOfWork.DeliveryMethodRepository.Add(delivery);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool deliveryMethodExists = await unitOfWork.DeliveryMethodRepository.Exists(x => x.Id == id);
            if(deliveryMethodExists)
            {
                await unitOfWork.DeliveryMethodRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The delivery method was not found...");
            }
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetAllAsync()
        {
            var deliveryMethodsDtos = new List<DeliveryMethodDto>();
            var deliveryMethods = await unitOfWork.DeliveryMethodRepository.GetAll();
            foreach(var deliveryMethod in deliveryMethods)
            {
                deliveryMethodsDtos.Add(mapper.Map<DeliveryMethodDto>(deliveryMethod));
            }
            return deliveryMethodsDtos;
        }

        public async Task<DeliveryMethodDto> GetByIdAsync(Guid id)
        {
            bool deliveryMethodExists = await unitOfWork.DeliveryMethodRepository.Exists(x => x.Id == id);
            if(deliveryMethodExists)
            {
               var deliveryMethod = await unitOfWork.DeliveryMethodRepository.Get(id);
                return mapper.Map<DeliveryMethodDto>(deliveryMethod);
            }
            throw new ItemNotFoundException("The delivery method was not found...");
        }
    }
}
