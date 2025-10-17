using BusinessLogic.DTOs;
using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services {
    public class CategoryService {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository) {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync() {
            var categories = await _categoryRepository.GetCategoriesAsync();
            return categories.Select(c => new CategoryDTO {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDescription = c.CategoryDescription,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategoryName,
                IsActive = c.IsActive,
                QuantityArticles = c.QuantityArticles
            }).ToList();
        }


        public async Task<Category> GetCategoryByIdAsync(int id)
                    => await _categoryRepository
            .GetCategoryByIdAsync(id);

        public async Task AddCategoryAsync(Category category) {
            var existing = await _categoryRepository
                .GetCategoryByNameAsync(category.CategoryName);
            if (existing != null) {
                throw new Exception("Category already exists!");
            }

            await _categoryRepository
                .AddCategoryAsync(category);
        }

        public async Task<IEnumerable<CategoryDTO>> SearchCategoriesAsync(string keyword) {
            var categories = await _categoryRepository.SearchAsync(keyword);
            return categories.Select(c => new CategoryDTO {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                CategoryDescription = c.CategoryDescription,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategoryName,
                IsActive = c.IsActive,
                QuantityArticles = c.QuantityArticles
            }).ToList();
            }

        public async Task UpdateCategoryAsync(Category category) {
            var existing = await _categoryRepository
                .GetCategoryByIdAsync(category.CategoryId);
            if (existing == null) {
                throw new Exception("Category not found!");
            }

            var existingName = await _categoryRepository
                .GetCategoryByNameAsync(category.CategoryName);
            if (existingName != null && existingName.CategoryId != category.CategoryId) {
                throw new Exception("Category name already exists!");
            }

            if (existing.ParentCategoryId != category.ParentCategoryId &&
                await _categoryRepository
                .HasArticlesAsync(category.CategoryId))
                throw new Exception("Cannot change parent category when articles exist.");
            await _categoryRepository
                .UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId) {
            var existing = await _categoryRepository
                .GetCategoryByIdAsync(categoryId);
            if (existing == null) {
                throw new Exception("Category not found!");
            }

            bool hasArticles = await _categoryRepository
                .HasArticlesAsync(categoryId);
            if (hasArticles)
                throw new Exception("Cannot delete category that contains articles.");

            await _categoryRepository
                .DeleteCategoryAsync(categoryId);
        }

        public Task ToggleCategoryActiveAsync(int categoryId) {
            throw new NotImplementedException();
        }
    }
}
