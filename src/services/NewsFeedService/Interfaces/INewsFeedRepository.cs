using NewsFeedService.DTOs;
using NewsFeedService.Entities;
using NewsFeedService.Services;

namespace NewsFeedService.Interfaces
{
    public interface INewsFeedRepository : IDisposable
    {
        Task<PaginatedFeed> GetFeeds(FeedsParameters feedsParameters);
    }
}
