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

        public async Task<Author?> GetNewsItems(int authorId)
        {
            return await this._dbContext.Authors.Include("NewsItems").Where(a => a.Id == authorId).FirstOrDefaultAsync();
        }
    }
}
