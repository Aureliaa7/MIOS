using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface IProductService
    {
        Task AddNewAsync(ProductCreationDto product);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        Task UpdateAsync(ProductEditingDto productDto);
        Task<ProductEditingDto> GetForUpdateAsync(string id);
        Task<IEnumerable<ProductDto>> GetByCategoryAsync(Guid categoryId);
        Task<int> GetNoProductsAsync();
        Task<IEnumerable<ProductDto>> Order(IEnumerable<ProductDto> products, string searchString, string orderCriteria);
    }
}
