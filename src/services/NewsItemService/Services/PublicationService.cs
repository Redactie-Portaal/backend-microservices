using NewsItemService.DTOs;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;
using RabbitMQLibrary;
using RabbitMQLibrary.Producer;

namespace NewsItemService.Services
{
    public class PublicationService
    {
        private readonly IPublicationRepository _publicationRepository;
        private readonly INewsItemRepository _newsItemRepostiory;
        private readonly IMediaNewsItemRepository _mediaNewsItemRepository;
        private readonly IMessageProducer _producer;

        public PublicationService(IPublicationRepository publicationRepository, INewsItemRepository newsItemRepository, IMediaNewsItemRepository mediaNewsItemRepository, IMessageProducer producer)
        {
            this._publicationRepository = publicationRepository;
            _newsItemRepostiory = newsItemRepository;
            this._mediaNewsItemRepository = mediaNewsItemRepository;
            _producer = producer;
        }

        public async Task<Dictionary<bool, string>> Publicize(int newsItemId, int publicationId)
        {
            var newsItem = (await _newsItemRepostiory.GetNewsItemById(newsItemId)).FirstOrDefault();

            if (!newsItem.Key)
            {
                return new Dictionary<bool, string>() { { false, "NEWSITEMNOTFOUND" } };
            }

            var publishDTO = new PublishDTO()
            {
                Content = newsItem.Value.Content,
                Summary = newsItem.Value.Summary,
            };

            if (newsItem.Value.Tags != null)
            {
                foreach (var item in newsItem.Value.Tags)
                {
                    publishDTO.Tags.Add(item.Name);
                }
            }

            var medias = await _mediaNewsItemRepository.GetMediaNewsItemById(newsItemId);

            if (medias.SingleOrDefault().Key)
            {
                var mediaDTOs = new List<MediaDTO>();
                foreach (var item in medias.SingleOrDefault().Value.Where(x => x.IsSource != true))
                {
                    mediaDTOs.Add(new MediaDTO() { FileID = item.MediaId, FileName = item.MediaFilename });
                }
                publishDTO.Media = mediaDTOs;
            }

            if (publicationId == 1)
            {
                _producer.PublishMessageAsync(RoutingKeyType.NewsItemPublishTwitter, publishDTO);
            }

            return new Dictionary<bool, string>() { { true, string.Empty } };
        }

        public async Task<Dictionary<bool, PublicationDTO>> GetPublication(int publicationId)
        {
            var result = await _publicationRepository.GetPublicationById(publicationId);
            if (!result.SingleOrDefault().Key)
            {
                return new Dictionary<bool, PublicationDTO>() { { false, null } };
            }

            var publicationDto = new PublicationDTO()
            {
                Id = result.SingleOrDefault().Value.Id,
                Name = result.SingleOrDefault().Value.Name,
                Description = result.SingleOrDefault().Value.Description,
                Icon = result.SingleOrDefault().Value.Icon
            };
            return new Dictionary<bool, PublicationDTO>() { { true, publicationDto } };
        }
    }
}
