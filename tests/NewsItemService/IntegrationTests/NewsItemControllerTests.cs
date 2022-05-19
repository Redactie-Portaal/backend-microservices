using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsItemService.Controllers;
using NewsItemService.Data;
using NewsItemService.Entities;
using NewsItemService.Services;
using NewsItemService.Types;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace NewsItemService
{
    public class NewsItemControllerTests
    {
        private NewsItemController Initialize(bool seed = true, [CallerMemberName] string callerName = "")
        {
            var options = new DbContextOptionsBuilder<NewsItemServiceDatabaseContext>().UseInMemoryDatabase(databaseName: "InMemoryProductDb_" + callerName).Options;
            var context = new NewsItemServiceDatabaseContext(options);
            if (seed)
            {
                SeedProductInMemoryDatabaseWithData(context);
            }
            var repo = new NewsItemRepository(context);


            var exchangeName = new ExchangeName("test-exchange");
            var connection = new RabbitMqConnection();

            var test = new MessageProducer(null, exchangeName,  connection);

            return new NewsItemController(test, repo);
        }

        private void SeedProductInMemoryDatabaseWithData(NewsItemServiceDatabaseContext context)
        {
            var newsItems = new List<NewsItem>
            {
                new NewsItem { Id = 1, Authors = null, Created = DateTime.Now, Updated = DateTime.Now, Status = NewsItemStatus.Publication}
            };

            if (!context.NewsItems.Any())
            {
                context.NewsItems.AddRange(newsItems);
            }
            
            context.SaveChanges();
        }

        [Fact]
        private async Task ChangeNewsItemStatus()
        {
            var controller = Initialize();

            var result = controller.AddNewsItemStatus(new DTOs.AddNewsItemStatusDTO { NewsItemId = 1, status = NewsItemStatus.Done });
            var resulttostring = result.Result as ObjectResult;

            var final = resulttostring.Value.GetType().GetProperty("message").GetValue(resulttostring.Value, null);
            Assert.Equal(final, "Status changed to Done");
        }
    }
}