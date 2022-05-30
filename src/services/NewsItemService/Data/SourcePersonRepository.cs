using Microsoft.EntityFrameworkCore;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class SourcePersonRepository : ISourcePersonRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public SourcePersonRepository(NewsItemServiceDatabaseContext context, ILogger<SourcePersonRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }

        public async Task<Dictionary<bool, SourcePerson>> GetSourcePerson(AddSourcePersonDTO addSourcePersonDTO)
        {
            try
            {
                var sourcePerson = await _dbContext.SourcePeople
                    .Where(p => p.Phone == addSourcePersonDTO.Phone)
                    .Where(n => n.Name == addSourcePersonDTO.Name).FirstOrDefaultAsync();
                if (sourcePerson == null)
                {
                    return new Dictionary<bool, SourcePerson>() { { false, null } };
                }
                return new Dictionary<bool, SourcePerson>() { { true, sourcePerson } };
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the SourcePerson. Error message: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Dictionary<bool, string>> CreateSourcePerson(SourcePerson sourcePerson)
        {
            try
            {
                var duplicate = await _dbContext.SourcePeople.FirstOrDefaultAsync(x => x.Id == sourcePerson.Id);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create SourcePerson because it is already present." } };
                }
                else
                {
                    await _dbContext.SourcePeople.AddAsync(sourcePerson);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with creating a SourcePerson. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"sourcePerson has been created succesfully" } };
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
