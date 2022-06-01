using PublicationService.DTOs;

namespace PublicationService.Interfaces
{
    public interface IPublicationService
    {
        Task<Dictionary<bool, string>> PublishNewsItem(PublishNewsItemDTO publishNewsItemDTO);
    }
}
