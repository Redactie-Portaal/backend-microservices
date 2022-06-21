using Microsoft.EntityFrameworkCore;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class SourceLocationRepository : ISourceLocationRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private readonly ILogger _logger;

        private bool _disposed = false;
        
        public SourceLocationRepository(NewsItemServiceDatabaseContext context, ILogger<SourceLocationRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<Dictionary<bool, SourceLocation>> GetSourceLocation(AddSourceLocationDTO addSourceLocationDTO)
        {
            try
            {
                var sourceLocation = await _dbContext.SourceLocations
                    .Where(p => p.PostalCode == addSourceLocationDTO.PostalCode)
                    .Where(h => h.HouseNumber == addSourceLocationDTO.HouseNumber)
                    .Where(c => c.Country == addSourceLocationDTO.Country).FirstOrDefaultAsync();
                if (sourceLocation == null)
                {
                    return new Dictionary<bool, SourceLocation>() { { false, null } };
                }
                return new Dictionary<bool, SourceLocation>() { { true, sourceLocation } };
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the SourceLocation. Error message: {Message}", ex.Message);
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
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with creating the SourceLocation. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"SourceLocation has been created succesfully" } };
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
