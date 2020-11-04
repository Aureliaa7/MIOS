using MusicalInstrumentsShop.BusinessLogic.DTOs;
using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task AddNewAsync(ProductCreationDto product, IEnumerable<Photo> photos);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(string id);
        Task<IEnumerable<string>> DeleteAsync(string id);
        Task<IEnumerable<string>> UpdateAsync(ProductEditingDto productDto, IEnumerable<Photo> photos);
        Task<ProductEditingDto> GetForUpdateAsync(string id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId);
    }
}
