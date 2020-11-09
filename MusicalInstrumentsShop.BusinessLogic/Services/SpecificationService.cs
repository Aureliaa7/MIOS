using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork unitOfWork;

        public SpecificationService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddAsync(SpecificationDto specificationDto)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == specificationDto.ProductId);
            if(productExists)
            {
                var product = await unitOfWork.ProductRepository.GetWithRelatedData(specificationDto.ProductId);
                var specification = new Specification
                {
                    Id = new Guid(),
                    Key = specificationDto.Key,
                    Value = specificationDto.Value,
                    Product = product
                };
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
                return new SpecificationDto
                {
                    Id = specification.Id,
                    Key = specification.Key,
                    Value = specification.Value
                };
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
                    var specificationDto = new SpecificationDto
                    {
                        Id = specification.Id,
                        Key = specification.Key,
                        Value = specification.Value,
                        ProductId = productId
                    };
                    specificationDtos.Add(specificationDto);
                }
                return specificationDtos;
            }
            throw new ItemNotFoundException("The product was not found...");
        }
    }
}
