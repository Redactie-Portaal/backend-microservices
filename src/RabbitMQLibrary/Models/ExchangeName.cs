using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary
{
    public class ExchangeName
    {
        public string Name { get; }
        public ExchangeName(string name)
        {
            Name = name;
        }
    }
}
