using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using RabbitMQLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add singletons
// Services
builder.Services.AddSingleton<NewsItemOverviewService>();
builder.Services.AddSingleton<AuthorService>();

// Repositories

builder.Services.AddScoped<INewsItemRepository, NewsItemRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();

// Contexts
builder.Services.AddSingleton<NewsItemServiceDatabaseContext>();

// Messaging
builder.Services.AddMessageProducing("news-item-exchange");

// Add the database context to the builder.
builder.Services.AddDbContext<NewsItemServiceDatabaseContext>();
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
