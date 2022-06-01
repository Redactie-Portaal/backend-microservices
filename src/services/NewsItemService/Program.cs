using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<INewsItemRepository, NewsItemRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IMediaNewsItemRepository, MediaNewsItemRepository>();
builder.Services.AddSingleton<IMediaRepository, MediaRepository>();
builder.Services.AddSingleton<INoteRepository, NoteRepository>();
builder.Services.AddSingleton<IPublicationRepository, PublicationRepository>();
builder.Services.AddSingleton<ISourceLocationRepository, SourceLocationRepository>();
builder.Services.AddSingleton<ISourcePersonRepository, SourcePersonRepository>();
builder.Services.AddSingleton<ITagRepository, TagRepository>();

// Add the database context to the builder.
builder.Services.AddSingleton<NewsItemServiceDatabaseContext>();
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
