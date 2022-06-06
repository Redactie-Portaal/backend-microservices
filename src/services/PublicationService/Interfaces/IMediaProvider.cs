namespace PublicationService.Interfaces
{
    public interface IMediaProvider
    {
        Task<Dictionary<string, byte[]>> RetrieveMedia(string fileName);
    }
}
