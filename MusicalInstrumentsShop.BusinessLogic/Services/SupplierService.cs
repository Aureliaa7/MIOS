using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Repositories.Interfaces;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IRepository<Supplier> supplierRepository;
        private readonly IProductRepository productRepository;
        private readonly IStockRepository stockRepository;
        private readonly IMapper mapper;

        public SupplierService(IRepository<Supplier> supplierRepository, IProductRepository productRepository,
            IStockRepository stockRepository, IMapper mapper)
        {
            this.supplierRepository = supplierRepository;
            this.productRepository = productRepository;
            this.stockRepository = stockRepository;
            this.mapper = mapper;
        }

        public async Task AddAsync(SupplierDto supplierDto)
        {
            var supplier = mapper.Map<Supplier>(supplierDto);
            await supplierRepository.Add(supplier);
        }

        public async Task<IEnumerable<string>> DeleteAsync(Guid id)
        {
            bool supplierExists = await supplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                var photoNamesToBeDeleted = new List<string>();
                var stocks = await stockRepository.GetBySupplierId(id);
                foreach(var stock in stocks)
                {
                    await stockRepository.Remove(stock.Id);
                    var photoNames = await productRepository.Delete(stock.Product.Id);
                    await supplierRepository.Remove(id);
                    photoNamesToBeDeleted.AddRange(photoNames);
                }
                
                return photoNamesToBeDeleted;
            }
            throw new ItemNotFoundException("The supplier was not found...");
        }

        public async Task<IEnumerable<SupplierDto>> GetAllAsync()
        {
            var suppliers = await supplierRepository.GetAll();
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
            bool supplierExists = await supplierRepository.Exists(x => x.Id == id);
            if (supplierExists)
            {
                var supplier = await supplierRepository.Get(id);
                return mapper.Map<SupplierDto>(supplier);
            }
            throw new ItemNotFoundException("The supplier was not found...");
        }

        public async Task<SupplierDto> GetByProductAsync(string productId)
        {
            bool productExists = await productRepository.Exists(x => x.Id == productId);
            if (productExists)
            {
                var stock = await stockRepository.GetByProductId(productId);
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
            await supplierRepository.Update(supplier);
        }
    }
}
