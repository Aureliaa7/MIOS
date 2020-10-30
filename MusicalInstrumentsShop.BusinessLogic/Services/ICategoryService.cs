using MusicalInstrumentsShop.BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ICategoryService
    {
        Task Add(CategoryDto category);
        Task<IEnumerable<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(Guid id);
        Task Delete(Guid id);
        Task Update(CategoryDto category);
    }
}
