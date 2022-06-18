using NewsItemService.DTOs;
using NewsItemService.Entities;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsItemService.Services
{
    public class PublicationService
    {
        private readonly INewsItemRepository _newsItemRepostiory;
        private readonly IMessageProducer _producer;
        private readonly IPublicationRepository _publicationRepository;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;

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

        public async Task<Dictionary<bool, PublicationDTO>> GetById(int id)
        {
            var publication = await _publicationRepository.GetPublicationById(id);
            if (publication.SingleOrDefault().Key)
            {
                var publicationDto = new PublicationDTO()
                {
                    Id = publication.SingleOrDefault().Value.Id,
                    Name = publication.SingleOrDefault().Value.Name,
                    Description = publication.SingleOrDefault().Value.Description,
                    Icon = publication.SingleOrDefault().Value.Icon
                };
                return new Dictionary<bool, PublicationDTO>() { { true, publicationDto } };
            }
            return new Dictionary<bool, PublicationDTO>() { { false, null } };
        }

        public async Task<Dictionary<bool, string>> PublishNewsItem(int newsItemId, int publicationId)
        {
            var newsItem = await _newsItemRepostiory.GetNewsItemById(newsItemId);

            if (!newsItem.SingleOrDefault().Key)
            {
                return new Dictionary<bool, string>() { { false, "NEWSITEMNOTFOUND" } };
            }

            var publishDTO = new PublishDTO()
            {
                Content = newsItem.SingleOrDefault().Value.Content,
                Summary = newsItem.SingleOrDefault().Value.Summary,
            };

            if (newsItem.SingleOrDefault().Value.Tags.Count != 0)
            {
                foreach (var item in newsItem.SingleOrDefault().Value.Tags)
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
