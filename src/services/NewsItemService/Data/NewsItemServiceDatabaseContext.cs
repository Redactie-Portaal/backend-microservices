using Microsoft.EntityFrameworkCore;
using NewsItemService.Entities;

namespace NewsItemService.Data
{
    public class NewsItemServiceDatabaseContext : DbContext
    {
        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class
        /// </summary>
        public NewsItemServiceDatabaseContext()
        {
        }

        /// <summary>
        /// Constructor of the NewsItemServiceDatabaseContext class with options, used for Unittesting
        /// Database options can be given, to switch between local and remote database
        /// </summary>
        /// <param name="options">Database options</param>
        public NewsItemServiceDatabaseContext(DbContextOptions<NewsItemServiceDatabaseContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet for the NewsItem class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<NewsItem> NewsItems { get; set; }
        /// <summary>
        /// DbSet for the Author class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Author> Authors { get; set; }
        /// <summary>
        /// DbSet for the Category class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>
        /// DbSet for the Tag class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// DbSet for the Publication class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Publication> Publications { get; set; }
        /// <summary>
        /// DbSet for the Media class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Media> Medias { get; set; }
        /// <summary>
        /// DbSet for the MediaNewsItem class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<MediaNewsItem> MediaNewsItems { get; set; }
        /// <summary>
        /// DbSet for the Note class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<Note> Notes { get; set; }
        /// <summary>
        /// DbSet for the Source_Location class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<SourceLocation> SourceLocations { get; set; }
        /// <summary>
        /// DbSet for the Source_Person class, A DbSet represents the collection of all entities in the context. 
        /// DbSet objects are created from a DbContext using the DbContext.Set method.
        /// </summary>
        public DbSet<SourcePerson> SourcePeople { get; set; }

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaNewsItem>().HasKey(a => new { a.MediaId, a.NewsItemId });
            modelBuilder.Entity<MediaNewsItem>().HasOne(x => x.Media).WithMany(y => y.MediaNewsItems).HasForeignKey(x => x.MediaId);
            modelBuilder.Entity<MediaNewsItem>().HasOne(x => x.NewsItem).WithMany(y => y.MediaNewsItems).HasForeignKey(x => x.NewsItemId);
        }
    }
}
