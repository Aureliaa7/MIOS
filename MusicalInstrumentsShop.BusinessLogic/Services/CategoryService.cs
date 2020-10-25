using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task Add(Category category)
        {
            await categoryRepository.Add(category);
        }

        public async Task Delete(Guid id)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == id);
            if(categoryExists)
            {
                await categoryRepository.Remove(id);
            }
            else
            {
                throw new ItemNotFoundException("The category was not found...");
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await categoryRepository.GetAll();
        }

        public async Task<Category> GetById(Guid id)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == id);
            if (categoryExists)
            {
                return await categoryRepository.Get(id);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task Update(Category category)
        {
            await categoryRepository.Update(category);
        }
    }
}
