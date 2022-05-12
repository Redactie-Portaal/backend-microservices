using NewsFeedService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsFeedService.Services
{
    public class PaginatedFeed
    {
        public PaginatedFeed(IEnumerable<Feed> items, int count, int pageNumber, int feedsPerPage)
        {
            PageInfo = new PageInfo
            {
                CurrentPage = pageNumber,
                PostsPerPage = feedsPerPage,
                TotalPages = (int)Math.Ceiling(count / (double)feedsPerPage),
                TotalPosts = count
            };
            this.Feeds = items;
        }

        public PageInfo PageInfo { get; set; }

        public IEnumerable<Feed> Feeds { get; set; }

        public static PaginatedFeed ToPaginatedPost(IQueryable<Feed> feeds, int pageNumber, int postsPerPage)
        {
            var count = feeds.Count();
            var chunk = feeds.Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage);
            return new PaginatedFeed(chunk, count, pageNumber, postsPerPage);
        }
    }

    public class PageInfo
    {
        public bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PostsPerPage { get; set; }
        public int TotalPosts { get; set; }
    }
}
