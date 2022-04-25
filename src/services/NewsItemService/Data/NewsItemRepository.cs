using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class NewsItemRepository : INewsItemRepository
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;

        public NewsItemRepository(NewsItemServiceDatabaseContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<NewsItem>> GetNewsItems(int authorId)
        {
            return await this._dbContext.NewsItems.Include(a => a.Authors.Where(i => i.Id == authorId)).ToListAsync();
        }
    }
}
