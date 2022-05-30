using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class SourcePersonRepository : ISourcePersonRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;

        public SourcePersonRepository(NewsItemServiceDatabaseContext context)
        {
            this._dbContext = context;
        }

        public async Task<Dictionary<bool, SourcePerson>> GetSourcePersonById(int id)
        {
            try
            {
                var sourcePerson = await _dbContext.SourcePeople.Where(a => a.Id == id).FirstOrDefaultAsync();
                if (sourcePerson == null)
                {
                    return new Dictionary<bool, SourcePerson>() { { false, null } };
                }
                return new Dictionary<bool, SourcePerson>() { { true, sourcePerson } };
            }
            catch (Exception)
            {
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
                    await Save();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"sourcePerson has been created succesfully" } };
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
