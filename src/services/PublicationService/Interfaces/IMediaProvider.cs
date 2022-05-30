namespace PublicationService.Interfaces
{
    public interface IMediaProvider
    {
        Task<Dictionary<bool, byte[]>> RetrieveMedia(string fileName);
    }
}
