using MusicalInstrumentsShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ICategoryService
    {
        Task Add(Category category);
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(Guid id);
        Task Delete(Guid id);
        Task Update(Category category);
    }
}
