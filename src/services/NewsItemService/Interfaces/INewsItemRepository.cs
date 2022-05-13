﻿using NewsItemService.Entities;
﻿using NewsItemService.DTOs;

namespace NewsItemService.Interfaces
{
    public interface INewsItemRepository : IDisposable
    {
        Task<Dictionary<bool, string>> CreateNewsItem(NewsItem item);
        Task<Dictionary<bool, Author>> GetAuthorById(int id);

        //TODO: remove this line that belongs to another branch
        Task<Dictionary<bool, string>> ChangeNewsItemStatus(AddNewsItemStatus newsItemStatus);
    }
}
