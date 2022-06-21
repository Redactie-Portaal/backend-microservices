using Microsoft.OpenApi.Models;
using NewsItemService.Data;
using NewsItemService.Interfaces;
using NewsItemService.Services;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        builder => builder.SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
        .Build();

// Add services to the container.
builder.Services.AddControllers();

// Messaging
builder.Services.AddMessageProducing("news-item-exchange");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsItemService", Version = "v1" });
});

builder.Services.AddMessageProducing("news-item-exchange");

// Add singletons
// Services
// Repositories
builder.Services.AddSingleton<INewsItemRepository, NewsItemRepository>();
builder.Services.AddSingleton<IAuthorRepository, AuthorRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();

builder.Services.AddSingleton<IMediaNewsItemRepository, MediaNewsItemRepository>();
builder.Services.AddSingleton<IMediaRepository, MediaRepository>();
builder.Services.AddSingleton<INoteRepository, NoteRepository>();
builder.Services.AddSingleton<IPublicationRepository, PublicationRepository>();
builder.Services.AddSingleton<ISourceLocationRepository, SourceLocationRepository>();
builder.Services.AddSingleton<ISourcePersonRepository, SourcePersonRepository>();
builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
builder.Services.AddSingleton<ITagRepository, TagRepository>();

builder.Services.AddSingleton<NewsItemOverviewService>();
builder.Services.AddSingleton<NewsItemsService>();
builder.Services.AddSingleton<NewsItemStatusService>();
builder.Services.AddSingleton<PublicationService>(); // Geen idee waarom maar AddSingleton werkte niet, dus daarom AddScoped;
builder.Services.AddSingleton<AuthorService>();
builder.Services.AddSingleton<TagService>();

// Messaging
builder.Services.AddMessageProducing("news-item-exchange");

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
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NewsItemService v1"));
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
