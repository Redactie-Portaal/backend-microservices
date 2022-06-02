using ArchiveService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArchiveService.Data
{
    public class ArchiveServiceDatabaseContext: DbContext
    {
        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class
        /// </summary>
        public ArchiveServiceDatabaseContext()
        {
        }

        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class with options, used for Unittesting
        /// Database options can be given, to switch between local and remote database
        /// </summary>
        /// <param name="options">Database options</param>
        public ArchiveServiceDatabaseContext(DbContextOptions<ArchiveServiceDatabaseContext> options) : base(options)
        {
        }

        public DbSet<TestingUnit> TestingUnits { get; set; }

        /// <summary>
        /// OnConfiguring builds the connection between the database and the API using the given connection string
        /// </summary>
        /// <param name="optionsBuilder">Used for adding options to the database to configure the connection.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var dbString = Environment.GetEnvironmentVariable("redactieportaal_db_string");
                if (string.IsNullOrWhiteSpace(dbString))
                {
                    throw new MissingFieldException("Database environment variable not found.");
                }

                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("redactieportaal_db_string").Replace("DATABASE_NAME", "archiveservice"));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}

