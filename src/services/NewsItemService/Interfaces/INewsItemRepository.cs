using NewsItemService.Entities;
﻿using NewsItemService.DTOs;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, NewsItem>> GetNewsItemById(int newsItemId);
        Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item);
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatusDTO newsItemStatus);
        Task<NewsItem> GetNewsItemAsync(int newsItemId);
    }
}
