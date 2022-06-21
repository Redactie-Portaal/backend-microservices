using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQLibrary
{
    public static class RoutingKeyType
    {
        public const string NewsItemCreated = "newsitem.created";
        public const string NewsItemUpdated = "newsitem.updated";
        public const string NewsItemDeleted = "newsitem.deleted";
        public const string NewsItemDispose = "newsitem.disposed";
        public const string NewsItemArchive = "newsitem.archived";
        public const string NewsItemPublishTwitter = "newsitem.publish.twitter";
    }
}
