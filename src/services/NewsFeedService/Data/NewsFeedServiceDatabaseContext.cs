using NewsFeedService.Entities;

namespace NewsFeedService.Data
{
    public class NewsFeedServiceDatabaseContext : DbContext
    {
        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class
        /// </summary>
        public NewsFeedServiceDatabaseContext()
        {
        }

        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class with options, used for Unittesting
        /// Database options can be given, to switch between local and remote database
        /// </summary>
        /// <param name="options">Database options</param>
        public NewsFeedServiceDatabaseContext(DbContextOptions<NewsFeedServiceDatabaseContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for the NewsItem class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Feed> Feeds { get; set; }
        /// <summary>
        /// DbSet for the Author class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<FeedActionHistory> FeedActionHistorys { get; set; }

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

                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("redactieportaal_db_string").Replace("DATABASE_NAME", "newsitemservice"));
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
}
