using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using AutoMapper;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SpecificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddAsync(SpecificationDto specificationDto)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == specificationDto.ProductId);
            if(productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedDataAsTracking(specificationDto.ProductId);
                var specification = mapper.Map<SpecificationDto, Specification>(specificationDto);
                specification.Product = product;
                await unitOfWork.SpecificationRepository.Add(specification);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The product was not found...");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            bool specificationExists = await unitOfWork.SpecificationRepository.Exists(x => x.Id == id);
            if(specificationExists)
            {
                await unitOfWork.SpecificationRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The specification was not found...");
            }
        }

        public async Task<SpecificationDto> GetByIdAsync(Guid id)
        {
            bool specificationExists = await unitOfWork.SpecificationRepository.Exists(x => x.Id == id);
            if(specificationExists)
            {
                var specification = await unitOfWork.SpecificationRepository.Get(id);
                return mapper.Map<Specification, SpecificationDto>(specification);
            }
            throw new ItemNotFoundException("The specification was not found...");
        }

        public async Task<IEnumerable<SpecificationDto>> GetForProductAsync(string productId)
        {

            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productId);
            if (productExists)
            {
                var specifications = await unitOfWork.SpecificationRepository.GetByProductId(productId);
                var specificationDtos = new List<SpecificationDto>();
                foreach(var specification in specifications)
                {
                    var specificationDto = mapper.Map<Specification, SpecificationDto>(specification);
                    specificationDto.ProductId = productId;
                    specificationDtos.Add(specificationDto);
                }
                return specificationDtos;
            }
            throw new ItemNotFoundException("The product was not found...");
        }
    }
}
