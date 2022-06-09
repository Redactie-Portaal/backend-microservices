using NewsItemService.DTOs;
using NewsItemService.Helpers;
using NewsItemService.Interfaces;

namespace NewsItemService.Services
{
    public class PublicationService
    {
        private readonly IPublicationRepository publicationRepository;
        private readonly INewsItemRepository _newsItemRepostiory;

        public PublicationService(IPublicationRepository publicationRepository, INewsItemRepository newsItemRepository)
        {
            this.publicationRepository = publicationRepository;
            _newsItemRepostiory = newsItemRepository;
        }

        public async Task<NewsItemDTO> Publicize(int newsItemId, int publicationId)
        {
            var newsItem = (await _newsItemRepostiory.GetNewsItemById(newsItemId)).FirstOrDefault();

            //Ugly
            if (newsItem.Key)
                return NewsItemHelper.ToDTO(newsItem.Value);
            else
                return null;
        }
    }
}
