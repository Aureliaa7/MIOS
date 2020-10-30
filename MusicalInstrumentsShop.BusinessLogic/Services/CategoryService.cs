﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicalInstrumentsShop.DataAccess.Repositories;
using MusicalInstrumentsShop.BusinessLogic.Exceptions;
using MusicalInstrumentsShop.DataAccess.Entities;
using MusicalInstrumentsShop.BusinessLogic.DTOs;
using AutoMapper;

namespace MusicalInstrumentsShop.BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IMapper mapper;

        public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        public async Task Add(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
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

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await categoryRepository.GetAll();
            var categoryDtos = new List<CategoryDto>();
            foreach(var category in categories)
            {
                categoryDtos.Add(mapper.Map<CategoryDto>(category));
            }
            return categoryDtos;
        }

        public async Task<CategoryDto> GetById(Guid id)
        {
            bool categoryExists = await categoryRepository.Exists(x => x.Id == id);
            if (categoryExists)
            {
                var category = await categoryRepository.Get(id);
                return mapper.Map<CategoryDto>(category);
            }
            throw new ItemNotFoundException("The category was not found...");
        }

        public async Task Update(CategoryDto categoryDto)
        {
            var category = mapper.Map<Category>(categoryDto);
            await categoryRepository.Update(category);
        }
    }
}
