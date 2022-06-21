using RabbitMQLibrary.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsItemService.Tests.UnitTests.Stubs
{
    internal class StubMessageProducer : IMessageProducer
    {
        public void PublishMessageAsync<T>(string routingKey, T message)
        {
            throw new NotImplementedException();
        }
    }
}
