using NewsItemService.Entities;

namespace NewsItemService.Interfaces
{
    public interface IMediaRepository
    {
        Task<Dictionary<bool, Media>> GetMediaByFilename(string fileName);
        Task<Dictionary<bool, string>> SaveMedia(Media media);
    }
}
