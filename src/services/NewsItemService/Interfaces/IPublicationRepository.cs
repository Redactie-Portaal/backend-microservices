using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IPublicationRepository
    {
        Task<Dictionary<bool, Publication>> GetPublicationById(int id);
    }
}
