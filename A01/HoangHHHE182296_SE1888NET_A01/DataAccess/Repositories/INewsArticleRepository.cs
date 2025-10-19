using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories {
    public interface INewsArticleRepository {
        Task<IEnumerable<NewsArticle>> SearchNewsArticleAsync(string? keyword, int? categoryId, int status);
        Task<NewsArticle> GetNewsArticleByIdAsync(int newsArticleId);
        Task AddNewsArticleAsync(NewsArticle newsArticle);
        Task UpdateNewsArticleAsync(NewsArticle newsArticle);
        Task DeleteNewsArticleAsync(int newsArticleId);
        Task<IEnumerable<NewsArticle>> GetNewsArticlesByAccountIdAsync(int accountId);
    }
}
