using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using AutoMapper;
using MusicalInstrumentsShop.BusinessLogic.Services.Interfaces;
using MusicalInstrumentsShop.DataAccess.UnitOfWork;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task AddAsync(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            await unitOfWork.CategoryRepository.Add(category);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == id);
            if(categoryExists)
            {
                await unitOfWork.CategoryRepository.Remove(id);
                await unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new ItemNotFoundException("The category was not found...");
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAll();
            var categoryDtos = new List<CategoryDto>();
            foreach(var category in categories)
            {
                categoryDtos.Add(mapper.Map<CategoryDto>(category));
            }
            return categoryDtos;
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            bool categoryExists = await unitOfWork.CategoryRepository.Exists(x => x.Id == id);
            if (categoryExists)
            {
                var category = await unitOfWork.CategoryRepository.Get(id);
                return mapper.Map<CategoryDto>(category);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task UpdateAsync(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
