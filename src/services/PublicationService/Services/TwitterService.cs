using PublicationService.Interfaces;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace PublicationService.Services
{
    public class TwitterService : IPublicationService
    {
        private readonly IMediaProvider _mediaProvider;
        public TwitterService(IMediaProvider mediaProvider)
        {
            this._mediaProvider = mediaProvider;
        }
        internal TwitterClient Authenticate()
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json").Build());
            var twitterClient = new TwitterClient(conf["Twitter:KEY"], conf["Twitter:SECRET"], conf["Twitter:ACCESS_TOKEN"], conf["Twitter:ACCESS_TOKEN_SECRET"]);
            return twitterClient;
        }

        public async Task<Dictionary<bool, string>> PublishStory(string text, string fileName)
        {
            var twitterClient = Authenticate();

            var picture = await this._mediaProvider.RetrieveMedia("");
            if (picture.SingleOrDefault().Key == false)
            {
                return new Dictionary<bool, string>() { { false, "Problem with retrieving the picture" } };
            }

            try
            {
                var uploadedImage = await twitterClient.Upload.UploadTweetImageAsync(picture.SingleOrDefault().Value);
                await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text)
                {
                    Medias = { uploadedImage }
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error with publishing the tweet. Message: {message}", exception.Message);
                throw;
            }

            return new Dictionary<bool, string>() { { true, string.Empty } };

        }
    }
}
