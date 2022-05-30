using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class SourceLocationRepository : ISourceLocationRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public SourceLocationRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, SourceLocation>> GetSourceLocationById(int id)
        {
            try
            {
                var sourceLocation = await _dbContext.SourceLocations.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (sourceLocation == null)
                {
                    return new Dictionary<bool, SourceLocation>() { { false, null } };
                }
                return new Dictionary<bool, SourceLocation>() { { true, sourceLocation } };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Dictionary<bool, string>> CreateSourceLocation(SourceLocation sourceLocation)
        {
            try
            {
                var duplicate = await _dbContext.SourceLocations.FirstOrDefaultAsync(x => x.Id == sourceLocation.Id);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create SourceLocation because it is already present." } };
                }
                else
                {
                    await _dbContext.SourceLocations.AddAsync(sourceLocation);
                    await Save();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"SourceLocation has been created succesfully" } };
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
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
