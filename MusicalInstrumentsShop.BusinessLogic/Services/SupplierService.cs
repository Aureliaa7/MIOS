using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddAsync(SupplierDto supplierDto)
        {
            var supplier = mapper.Map<Supplier>(supplierDto);
            await unitOfWork.SupplierRepository.Add(supplier);
        }

        public async Task<IEnumerable<string>> DeleteAsync(Guid id)
        {
            bool supplierExists = await unitOfWork.SupplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                var photoNamesToBeDeleted = new List<string>();
                var stocks = await unitOfWork.StockRepository.GetBySupplierId(id);
                foreach(var stock in stocks)
                {
                    await unitOfWork.StockRepository.Remove(stock.Id);
                    var photoNames = await unitOfWork.ProductRepository.Delete(stock.Product.Id);
                    photoNamesToBeDeleted.AddRange(photoNames);
                }
                await unitOfWork.SupplierRepository.Remove(id);
                return photoNamesToBeDeleted;
            }
            throw new ItemNotFoundException("The supplier was not found...");
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

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            var supplier = mapper.Map<Supplier>(supplierDto);
            await unitOfWork.SupplierRepository.Update(supplier);
        }
    }
}
