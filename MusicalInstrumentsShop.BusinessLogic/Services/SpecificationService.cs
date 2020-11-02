using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationRepository specificationRepository;
        private readonly IProductRepository productRepository;

        public SpecificationService(ISpecificationRepository specificationRepository, IProductRepository productRepository)
        {
            this.specificationRepository = specificationRepository;
            this.productRepository = productRepository;
        }

        public async Task Add(SpecificationDto specificationDto)
        {
            bool productExists = await productRepository.Exists(x => x.Id == specificationDto.ProductId);
            if(productExists)
            {
                var product = await productRepository.GetWithRelatedData(specificationDto.ProductId);
                var specification = new Specification
                {
                    Id = new Guid(),
                    Key = specificationDto.Key,
                    Value = specificationDto.Value,
                    Product = product
                };
                await specificationRepository.Add(specification);
            }
            else
            {
                throw new ItemNotFoundException("The product was not found...");
            }
        }

        public async Task Delete(Guid id)
        {
            bool specificationExists = await specificationRepository.Exists(x => x.Id == id);
            if(specificationExists)
            {
                await specificationRepository.Remove(id);
            }
            else
            {
                throw new ItemNotFoundException("The specification was not found...");
            }
        }

        public async Task<SpecificationDto> GetById(Guid id)
        {
            bool specificationExists = await specificationRepository.Exists(x => x.Id == id);
            if(specificationExists)
            {
                var specification = await specificationRepository.Get(id);
                return new SpecificationDto
                {
                    Id = specification.Id,
                    Key = specification.Key,
                    Value = specification.Value
                };
            }
            throw new ItemNotFoundException("The specification was not found...");
        }

        public async Task<IEnumerable<SpecificationDto>> GetForProduct(string productId)
        {

            bool productExists = await productRepository.Exists(x => x.Id == productId);
            if (productExists)
            {
                var specifications = await specificationRepository.GetByProductId(productId);
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
