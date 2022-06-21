using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class TagRepository : ITagRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public TagRepository(NewsItemServiceDatabaseContext context, ILogger<TagRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the Tag. Error message: {Message}", ex.Message);
                throw;
            }
        }

        public Tag Post(Tag tag)
        {
            _dbContext.Tags.Add(tag);
            _dbContext.SaveChanges();

            return tag;
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
