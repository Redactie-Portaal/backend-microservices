using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;

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
builder.Services.AddSingleton<INewsItemRepository, NewsItemRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();

// Contexts
builder.Services.AddSingleton<NewsItemServiceDatabaseContext>();


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

// Ensure database(s?) created
var newsItemServiceContext = builder.Services.BuildServiceProvider().GetService<NewsItemServiceDatabaseContext>();
newsItemServiceContext?.Database.EnsureCreated();

app.Run();
