using NewsItemService.DTOs;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class PublicationService
    {
        private readonly IPublicationRepository publicationRepository;
        private readonly IMediaRepository _mediaRepository;
        private readonly INewsItemRepository _newsItemRepostiory;

        public PublicationService(IPublicationRepository publicationRepository, IMediaRepository mediaRepository, INewsItemRepository newsItemRepository)
        {
            this.publicationRepository = publicationRepository;
            _mediaRepository = mediaRepository;
            _newsItemRepostiory = newsItemRepository;
        }

        public async Task<PublishDTO> Publicize(int newsItemId, int publicationId)
        {
            var newsItem = (await _newsItemRepostiory.GetNewsItemById(newsItemId)).FirstOrDefault();

            //Ugly
            if (newsItem.Key)
            {
                var media = new List<MediaDTO>();
                foreach (var m in newsItem.Value.MediaNewsItems)
                {
                    media.Add(new MediaDTO()
                    {
                        FileID = m.Id,
                        FileName = m.MediaFilename
                    });
                }
                return new PublishDTO()
                {
                    Content = newsItem.Value.Content,
                    Summary = newsItem.Value.Summary,
                    Media = media,
                    Tags = newsItem.Value.Tags.ToList()
                };
            }
            else
                return null;
        }
    }
}
