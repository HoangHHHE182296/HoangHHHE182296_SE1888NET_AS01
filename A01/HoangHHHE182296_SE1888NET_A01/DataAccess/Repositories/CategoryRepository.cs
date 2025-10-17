using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories {

    public class CategoryRepository : ICategoryRepository {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesAsync() =>
             await _dbContext.Categories.Select(c => new CategoryResponse {
                 CategoryId = c.CategoryId,
                 CategoryName = c.CategoryName,
                 CategoryDescription = c.CategoryDesciption,
                 ParentCategoryId = c.ParentCategoryId,
                 ParentCategoryName = c.ParentCategory.CategoryName,
                 IsActive = c.IsActive,
                 QuantityArticles = c.NewsArticles.Count
             }).ToListAsync();

        public async Task AddCategoryAsync(Category category) {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId) =>
            await _dbContext.Categories.Where(c => c.CategoryId == categoryId).ExecuteDeleteAsync();


        public async Task<Category> GetCategoryByIdAsync(int categoryId) =>
             await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);


        public async Task<Category> GetCategoryByNameAsync(string categoryName) => await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);

        public async Task UpdateCategoryAsync(Category category) {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasArticlesAsync(int categoryId)
            => await _dbContext.NewsArticles.AnyAsync(a => a.CategoryId == categoryId);

        public async Task<IEnumerable<CategoryResponse>> SearchAsync(string keyword) {
            keyword = keyword.ToLower();
            return await _dbContext.Categories
                .Where(c => c.CategoryName.ToLower().Contains(keyword) ||
                            c.CategoryDesciption.ToLower().Contains(keyword))
                .Select(c => new CategoryResponse {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName,
                    CategoryDescription = c.CategoryDesciption,
                    ParentCategoryId = c.ParentCategoryId,
                    ParentCategoryName = c.ParentCategory.CategoryName,
                    IsActive = c.IsActive,
                    QuantityArticles = c.NewsArticles.Count
                }).ToListAsync();
        }
    }
}
