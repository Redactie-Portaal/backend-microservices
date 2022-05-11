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

        public async Task<List<NewsItem>?> GetNewsItems(int authorId)
        {
            //return await this._dbContext.Authors.Include("NewsItems").Where(a => a.Id == authorId).FirstOrDefaultAsync();
            if (await this._dbContext.Authors.Where(a => a.Id == authorId).SingleOrDefaultAsync() == null) return null;
            return await this._dbContext.NewsItems.Where(n => n.Authors.Where(a => a.Id == authorId).FirstOrDefault() != null).Include("Authors").Take(20).ToListAsync();
        }
    }
}
