using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IMediaNewsItemRepository, MediaNewsItemRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();
builder.Services.AddScoped<ISourceLocationRepository, SourceLocationRepository>();
builder.Services.AddScoped<ISourcePersonRepository, SourcePersonRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// Add the database context to the builder.
builder.Services.AddDbContext<NewsItemServiceDatabaseContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); // Needed for saving DateTime variables
using var newsItemContext = new NewsItemServiceDatabaseContext();
newsItemContext.Database.EnsureCreated();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
