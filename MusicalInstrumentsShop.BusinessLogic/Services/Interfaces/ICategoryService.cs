using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryDto category);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(CategoryDto category);
        Task<int> GetNoCategoriesAsync();
    }
}
