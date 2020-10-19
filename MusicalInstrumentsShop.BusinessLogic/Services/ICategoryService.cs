using MusicalInstrumentsShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public interface ICategoryService
    {
        public Task Add(Category category);
        public Task<IEnumerable<Category>> GetAll();
        public Task<Category> GetById(Guid id);
        public Task Delete(Guid id);
        public Task Update(Category category);
    }
}
