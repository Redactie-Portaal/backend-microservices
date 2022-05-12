using NewsFeedService.Interfaces;

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
