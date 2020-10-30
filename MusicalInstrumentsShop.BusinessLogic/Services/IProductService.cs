using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface IProductService
    {
        Task AddNew(ProductCreationDto product, IEnumerable<Photo> photos);
        Task<IEnumerable<ProductDto>> GetAll();
        Task<ProductDto> GetById(string id);
        Task<IEnumerable<string>> Delete(string id);
        Task<IEnumerable<string>> Update(ProductEditingDto productDto, IEnumerable<Photo> photos);
        Task<ProductEditingDto> GetForUpdate(string id);
        Task<IEnumerable<ProductDto>> GetByCategory(Guid categoryId);
    }
}
