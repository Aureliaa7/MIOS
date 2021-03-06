﻿using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;
using System.Linq;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IImageService imageService;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.imageService = imageService;
        }

        public async Task AddAsync(SupplierDto supplierDto)
        {
            var supplier = mapper.Map<Supplier>(supplierDto);
            await unitOfWork.SupplierRepository.Add(supplier);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                var photoNamesToBeDeleted = new List<string>();
                var stocks = await unitOfWork.StockRepository.GetBySupplierId(id);
                foreach (var stock in stocks)
                {
                    await unitOfWork.StockRepository.Remove(stock.Id);
                    var photoNames = await unitOfWork.ProductRepository.Delete(stock.Product.Id);
                    photoNamesToBeDeleted.AddRange(photoNames);
                }
                await unitOfWork.SupplierRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
                await imageService.DeleteFiles(photoNamesToBeDeleted);
            }
            else
            {
                throw new ItemNotFoundException("The supplier was not found...");
            }
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var suppliers = await unitOfWork.SupplierRepository.GetAll();
            var supplierDtos = new List<SupplierDto>();
            foreach(var supplier in suppliers)
            {
                var supplierDto = mapper.Map<SupplierDto>(supplier);
                supplierDtos.Add(supplierDto);
            }
            return supplierDtos;
        }

        public async Task<SupplierDto> GetByIdAsync(Guid id)
        {
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                var supplier = await unitOfWork.SupplierRepository.Get(id);
                return mapper.Map<SupplierDto>(supplier);
            }
            throw new ItemNotFoundException("The supplier was not found...");
        }

        public async Task<SupplierDto> GetByProductAsync(string productId)
        {
            bool productExists = await unitOfWork.ProductRepository.Exists(x => x.Id == productId);
            if (productExists)
            {
                var stock = await unitOfWork.StockRepository.GetByProductId(productId);
                if(stock != null)
                {
                    return mapper.Map<SupplierDto>(stock.Supplier);
                }
            }
            throw new ItemNotFoundException("The product was not found");
        }

        public async Task<int> GetNoSuppliersAsync()
        {
            var suppliers = await unitOfWork.SupplierRepository.GetAll();
            return suppliers.Count();
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == supplierDto.Id);
            if (supplierExists)
            {
                var supplier = mapper.Map<Supplier>(supplierDto);
                unitOfWork.SupplierRepository.Update(supplier);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The supplier was not found...");
            }
        }
    }
}
