using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class TagRepository : ITagRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public TagRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, Tag>> GetTagById(int id)
        {
            try
            {
                var tag = await _dbContext.Tags.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (tag == null)
                {
                    return new Dictionary<bool, Tag>() { { false, null } };
                }
                return new Dictionary<bool, Tag>() { { true, tag } };
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
