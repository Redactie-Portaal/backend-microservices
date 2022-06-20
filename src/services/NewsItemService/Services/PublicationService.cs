using NewsItemService.DTOs;
using NewsItemService.Interfaces;
using NewsItemService.Types;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsItemService.Services
{
    public class PublicationService
    {
        private readonly INewsItemRepository _newsItemRepostiory;
        private readonly IPublicationRepository _publicationRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly IMessageProducer _producer;

        public PublicationService(INewsItemRepository newsItemRepository,
                                  IPublicationRepository publicationRepository,
                                  IMediaNewsItemRepository mediaNewsItemRepository,
                                  IMessageProducer producer)
        {
            _newsItemRepostiory = newsItemRepository;
            _publicationRepository = publicationRepository;
            _mediaNewsItemRepository = mediaNewsItemRepository;
            _producer = producer;
        }

        public async Task<Dictionary<bool, PublicationDTO?>> GetById(int id)
        {
            var publication = await _publicationRepository.GetPublicationById(id);
            if (publication.SingleOrDefault().Key)
            {
                var p = publication.SingleOrDefault().Value;
                var publicationDto = new PublicationDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Icon = p.Icon
                };
                return new Dictionary<bool, PublicationDTO?>() { { true, publicationDto } };
            }
            return new Dictionary<bool, PublicationDTO?>() { { false, null } };
        }

        public async Task<Dictionary<bool, string>> PublishNewsItem(int newsItemId, int publicationId)
        {
            var newsItem = _newsItemRepostiory.Get(newsItemId);
            if (newsItem == null) return new Dictionary<bool, string>() { { false, ErrorType.NEWS_ITEM_NOT_FOUND} };

            var publishDTO = new PublishDTO()
            {
                Content = newsItem.Content,
                Summary = newsItem.Summary,
            };
            if (newsItem.Tags.Count != 0)
            {
                foreach (var item in newsItem.Tags)
                {
                    publishDTO.Tags.Add(item.Name);
                }
            }

            var medias = await _mediaNewsItemRepository.GetMediaNewsItemByNewsItemId(newsItemId);

            if (medias.SingleOrDefault().Key)
            {
                var mediaDTOs = new List<MediaDTO>();
                foreach (var item in medias.SingleOrDefault().Value.Where(x => x.IsSource != true))
                {
                    mediaDTOs.Add(new MediaDTO() { FileID = item.MediaId, FileName = item.MediaFilename });
                }
                publishDTO.Media = mediaDTOs;
            }

            _producer.PublishMessageAsync(RoutingKeyType.NewsItemPublishTwitter, publishDTO);
            return new Dictionary<bool, string>() { { true, string.Empty } };
        }
    }
}
