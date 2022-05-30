using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsItemService.Data;
using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class NewsItemsService
    {
        private readonly ModelStateDictionary modelState = new ModelStateDictionary();
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly ISourceLocationRepository _sourceLocationRepository;
        private readonly ISourcePersonRepository _sourcePersonRepository;

        public NewsItemsService(INewsItemRepository repo, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, IPublicationRepository publicationRepository, ITagRepository tagRepository, IMediaRepository mediaRepository, IMediaNewsItemRepository mediaNewsItemRepository, ISourceLocationRepository sourceLocationRepository, ISourcePersonRepository sourcePersonRepository)
        {
            _newsItemRepository = repo;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _publicationRepository = publicationRepository;
            _tagRepository = tagRepository;
            _mediaRepository = mediaRepository;
            _mediaNewsItemRepository = mediaNewsItemRepository;
            _sourceLocationRepository = sourceLocationRepository;
            _sourcePersonRepository = sourcePersonRepository;
        }

        public async Task<Dictionary<bool, string>> CreateNewsItem(CreateNewsItemDTO dto)
        {
            List<Author> authors = new();
            foreach (var id in dto.AuthorIds)
            {
                var author = await _authorRepository.GetAuthorById(id);
                if (author.FirstOrDefault().Key == false)
                {
                    return new Dictionary<bool, string>() { { false, "Author does not exist" } };
                }
                authors.Add(author.FirstOrDefault().Value);
            }

            List<Category> categories = new();
            if (dto.CategoryIds.Count != 0)
            {
                foreach (var id in dto.CategoryIds)
                {
                    var category = await _categoryRepository.GetCategoryById(id);
                    if (category.FirstOrDefault().Key == false)
                    {
                        return new Dictionary<bool, string>() { { false, "Category does not exist" } };
                    }
                    categories.Add(category.FirstOrDefault().Value);
                }
            }

            List<Tag> tags = new();
            if (dto.TagIds.Count != 0)
            {
                foreach (var id in dto.TagIds)
                {
                    var tag = await _tagRepository.GetTagById(id);
                    if (tag.FirstOrDefault().Key == false)
                    {
                        return new Dictionary<bool, string>() { { false, "Tag does not exist" } };
                    }
                    tags.Add(tag.FirstOrDefault().Value);
                }
            }

            List<Publication> publications = new();
            if (dto.PublicationIds.Count != 0)
            {
                foreach (var id in dto.PublicationIds)
                {
                    var publication = await _publicationRepository.GetPublicationById(id);
                    if (publication.FirstOrDefault().Key == false)
                    {
                        return new Dictionary<bool, string>() { { false, "Tag does not exist" } };
                    }
                    publications.Add(publication.FirstOrDefault().Value);
                }
            }


            //TODO: add ${MediaDTO}, ${SourceLocationDTO}, ${SourcePersonDTO}, ${NoteDTO}

            var newsItem = new NewsItem()
            {
                Content = dto.Content,
                Title = dto.Title,
                Authors = authors,
                Status = Types.NewsItemStatus.Done,
                Created = dto.ProductionDate.ToUniversalTime(),
                EndDate = dto.EndDate.ToUniversalTime(),
                Categories = categories,
                Tags = tags,
                Publications = publications
            };
            try
            {
                return await _newsItemRepository.CreateNewsItem(newsItem);

                //TODO: The note will be saved, after creating the news item.
                //TODO: Add reference of new item to MediaNewsItem, after creating the news item.
            }
            catch (Exception)
            {
                throw; //Something
            }
        }
    }
}
