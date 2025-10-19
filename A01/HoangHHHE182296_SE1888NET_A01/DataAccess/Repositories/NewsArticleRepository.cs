using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories {
    public class NewsArticleRepository : INewsArticleRepository {
        private readonly ApplicationDbContext _dbContext;

        public NewsArticleRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Task AddNewsArticleAsync(NewsArticle newsArticle) {
            throw new NotImplementedException();
        }

        public Task DeleteNewsArticleAsync(int newsArticleId) {
            throw new NotImplementedException();
        }

        public Task<NewsArticle> GetNewsArticleByIdAsync(int newsArticleId) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsArticlesByAccountIdAsync(int accountId) {
            return await _dbContext.NewsArticles.Where(a => a.CreatedById == accountId).ToListAsync();
        }

        public Task<IEnumerable<NewsArticle>> SearchNewsArticleAsync(string? keyword, int? categoryId, int status) {
            throw new NotImplementedException();
        }

        public Task UpdateNewsArticleAsync(NewsArticle newsArticle) {
            throw new NotImplementedException();
        }
    }
}
