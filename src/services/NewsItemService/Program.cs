using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using RabbitMQLibrary;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMessageProducing("news-item-exchange");
builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

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
