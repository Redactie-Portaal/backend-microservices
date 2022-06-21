using NewsItemService.Entities;
using NewsItemService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsItemService.Tests.UnitTests.Stubs
{
    internal class StubMediaNewsItemRepository : IMediaNewsItemRepository
    {
        public Task<Dictionary<bool, string>> CreateMediaNewsItem(MediaNewsItem mediaNewsItem)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<bool, List<MediaNewsItem>>> GetMediaNewsItemByNewsItemId(int newsItemId)
        {
            throw new NotImplementedException();
        }
    }
}
