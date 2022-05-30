namespace PublicationService.Interfaces
{
    public interface IPublicationService
    {
        Task<Dictionary<bool, string>> PublishStory(string text, string fileName);
    }
}
