using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories {
    internal class CategoryRepository : ICategoryRepository {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync() {
            return await _dbContext.Category.ToListAsync();
        }

        public async Task AddCategoryAsync(Category category) {
            await _dbContext.Category.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int categoryId) {
            await _dbContext.Category.Where(c => c.CategoryId == categoryId).ExecuteDeleteAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId) {
            return await _dbContext.Category.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task UpdateCategoryAsync(Category category) {
            _dbContext.Category.Update(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
