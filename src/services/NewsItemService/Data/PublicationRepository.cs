using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class PublicationRepository : IPublicationRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public PublicationRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, Publication>> GetPublicationById(int id)
        {
            try
            {
                var publication = await _dbContext.Publications.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (publication == null)
                {
                    return new Dictionary<bool, Publication>() { { false, null } };
                }
                return new Dictionary<bool, Publication>() { { true, publication } };
            }
            catch (Exception)
            {
                throw;
            }
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
            GC.SuppressFinalize(this);
        }
    }
}
