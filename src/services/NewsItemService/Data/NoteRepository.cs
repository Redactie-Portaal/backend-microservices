using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class NoteRepository : INoteRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private bool disposed = false;
        private readonly ILogger _logger;

        public NoteRepository(NewsItemServiceDatabaseContext context, ILogger<NoteRepository> logger)
        {
            this._dbContext = context;
            this._logger = logger;
        }

        public async Task<Dictionary<bool, string>> CreateNote(Note note)
        {
            try
            {
                var duplicate = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Text == note.Text);

                if (duplicate != null)
                {
                    return new Dictionary<bool, string>() { { false, "Can't create note because it is already present." } };
                }
                else
                {
                    await _dbContext.Notes.AddAsync(note);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the Note. Error message: {Message}", ex.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { false, $"Note has been created succesfully" } };
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
