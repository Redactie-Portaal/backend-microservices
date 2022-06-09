using PublicationService.DTOs;

namespace PublicationService.Interfaces
{
    public interface IPublicationService
    {
        Task PublishNewsItem(PublishNewsItemDTO publishNewsItemDTO);
    }
}
