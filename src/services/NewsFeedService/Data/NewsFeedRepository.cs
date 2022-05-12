using NewsFeedService.DTOs;
using NewsFeedService.Entities;
using NewsFeedService.Interfaces;
using NewsFeedService.Services;

namespace NewsFeedService.Data
{
    public class NewsFeedRepository : INewsFeedRepository, IDisposable
    {
        private readonly NewsFeedServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public NewsFeedRepository(NewsFeedServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<PaginatedFeed> GetFeeds(FeedsParameters feedsParameters)
        {
            if (feedsParameters.PageNumber == default || feedsParameters.PageSize == default || feedsParameters == default)
            {
                return null;
            }
            return PaginatedFeed.ToPaginatedPost(_dbContext.Feeds.OrderBy(x => x.Id), feedsParameters.PageNumber, feedsParameters.PageSize);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
