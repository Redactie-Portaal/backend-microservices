﻿using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class PublicationRepository : IPublicationRepository, IDisposable
    {
        private readonly NewsItemServiceDatabaseContext _dbContext;
        private readonly ILogger _logger;

        private bool _disposed = false;

        public PublicationRepository(NewsItemServiceDatabaseContext context, ILogger<PublicationRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
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
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with retrieving the Publication. Error message: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Publication> Create(Publication publication)
        {
            try
            {
                await _dbContext.Publications.AddAsync(publication);
                await _dbContext.SaveChangesAsync();

                return publication;
            }
            catch (Exception ex)
            {
                _logger.LogError("There is a problem with creating the Publication. Error message: {Message}", ex.Message);
                throw;
            }
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
