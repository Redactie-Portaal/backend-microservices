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
        private readonly INewsItemRepository _newsItemRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly ISourceLocationRepository _sourceLocationRepository;
        private readonly ISourcePersonRepository _sourcePersonRepository;
        private readonly INoteRepository _noteRepository;

        public NewsItemsService(INewsItemRepository repo,
                                IAuthorRepository authorRepository,
                                ICategoryRepository categoryRepository,
                                IPublicationRepository publicationRepository,
                                ITagRepository tagRepository,
                                IMediaRepository mediaRepository,
                                IMediaNewsItemRepository mediaNewsItemRepository,
                                ISourceLocationRepository sourceLocationRepository,
                                ISourcePersonRepository sourcePersonRepository,
                                INoteRepository noteRepository)
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
            _noteRepository = noteRepository;
        }

        public async Task<Dictionary<bool, string>> CreateNewsItem(CreateNewsItemDTO dto)
        {
            List<Author> authors = new();
            foreach (var id in dto.AuthorIds)
            {
                var author = await _authorRepository.GetAuthorById(id);
                if (!author.FirstOrDefault().Key)
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
                    if (!category.FirstOrDefault().Key)
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
                    if (!tag.FirstOrDefault().Key)
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
                    if (!publication.FirstOrDefault().Key)
                    {
                        return new Dictionary<bool, string>() { { false, "Tag does not exist" } };
                    }
                    publications.Add(publication.FirstOrDefault().Value);
                }
            }

            List<SourceLocation> sourceLocations = new();
            if (dto.SourceLocationDTOs.Count != 0)
            {
                foreach (var sourceLocale in dto.SourceLocationDTOs)
                {
                    var sourceLocation = await _sourceLocationRepository.GetSourceLocation(sourceLocale);
                    if (!sourceLocation.FirstOrDefault().Key)
                    {
                        SourceLocation newSourceLocation = new SourceLocation()
                        {
                            StreetName = sourceLocale.StreetName,
                            HouseNumber = sourceLocale.HouseNumber,
                            PostalCode = sourceLocale.PostalCode,
                            City = sourceLocale.City,
                            Province = sourceLocale.Province,
                            Country = sourceLocale.Country
                        };
                        await _sourceLocationRepository.CreateSourceLocation(newSourceLocation);
                        var newlyStoredSouceLocation = await _sourceLocationRepository.GetSourceLocation(sourceLocale);
                        sourceLocations.Add(newlyStoredSouceLocation.SingleOrDefault().Value);
                    }
                    else
                    {
                        sourceLocations.Add(sourceLocation.SingleOrDefault().Value);
                    }
                }
            }

            List<SourcePerson> sourcePeople = new();
            if (dto.SourcePersonDTOs.Count != 0)
            {
                foreach (var sourcePersona in dto.SourcePersonDTOs)
                {
                    var sourcePerson = await _sourcePersonRepository.GetSourcePerson(sourcePersona);
                    if (!sourcePerson.FirstOrDefault().Key)
                    {
                        SourcePerson newSourcePerson = new SourcePerson()
                        {
                            Name = sourcePersona.Name,
                            Email = sourcePersona.Email,
                            Phone = sourcePersona.Phone
                        };
                        await _sourcePersonRepository.CreateSourcePerson(newSourcePerson);
                        var newlyStoredSourcePerson = await _sourcePersonRepository.GetSourcePerson(sourcePersona);
                        sourcePeople.Add(newlyStoredSourcePerson.SingleOrDefault().Value);
                    }
                    else
                    {
                        sourcePeople.Add(sourcePerson.SingleOrDefault().Value);
                    }
                }
            }

            List<MediaNewsItem> mediaNewsItems = new();
            if (dto.MediaDTOs.Count != 0)
            {
                foreach (var medium in dto.MediaDTOs)
                {
                    var media = await _mediaRepository.GetMediaByFilename(medium.FileName);
                    if (!media.FirstOrDefault().Key)
                    {
                        Media newMedia = new Media()
                        {
                            FileName = medium.FileName,
                            FileContent = Convert.FromBase64String(medium.FileContent)
                        };
                        var newFile = await _mediaRepository.SaveMedia(newMedia);
                        var newMediaNewsItem = new MediaNewsItem() { MediaId = newFile.SingleOrDefault().Value, IsSource = medium.IsSource };
                        mediaNewsItems.Add(newMediaNewsItem);
                    }
                    else
                    {
                        var newMediaNewsItem = new MediaNewsItem() { MediaId = media.SingleOrDefault().Value.Id, IsSource = medium.IsSource };
                        mediaNewsItems.Add(newMediaNewsItem);
                    }
                }
            }

            List<Note> notes = new();
            if (dto.NoteDTO != null)
            {
                var author = await _authorRepository.GetAuthorById(dto.NoteDTO.AuthorId);
                if (!author.FirstOrDefault().Key)
                {
                    return new Dictionary<bool, string>() { { false, "Author does not exist" } };
                }
                Note note = new Note();
                note.Author = author.FirstOrDefault().Value;
                note.Text = dto.NoteDTO.Text;
                note.Updated = dto.NoteDTO.Updated;

                notes.Add(note);
            }

            var newsItem = new NewsItem()
            {
                Content = dto.Content,
                Summary = dto.Summary,
                Title = dto.Title,
                Authors = authors,
                Status = Types.NewsItemStatus.Done,
                Created = dto.ProductionDate.ToUniversalTime(),
                EndDate = dto.EndDate.ToUniversalTime(),
                Categories = categories,
                Tags = tags,
                Publications = publications,
                SourceLocations = sourceLocations,
                SourcePeople = sourcePeople,
                Notes = notes,
                MediaNewsItems = mediaNewsItems
            };

            try
            {
                return await _newsItemRepository.CreateNewsItem(newsItem);
            }
            catch (Exception)
            {
                throw; //Something
            }
        }
    }
}
