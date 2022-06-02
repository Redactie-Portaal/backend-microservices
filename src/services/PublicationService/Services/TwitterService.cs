using PublicationService.DTOs;
using PublicationService.Interfaces;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace PublicationService.Services
{
    public class TwitterService : IPublicationService
    {
        private readonly IMediaProvider _mediaProvider;
        private readonly ILogger _logger;

        public TwitterService(IMediaProvider mediaProvider, ILogger<IPublicationService> logger)
        {
            this._mediaProvider = mediaProvider;
            this._logger = logger;
        }

        internal TwitterClient Authenticate()
        {
            IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("secrets.json").Build());
            var twitterClient = new TwitterClient(conf["Twitter:KEY"], conf["Twitter:SECRET"], conf["Twitter:ACCESS_TOKEN"], conf["Twitter:ACCESS_TOKEN_SECRET"]);
            return twitterClient;
        }

        public async Task<Dictionary<bool, string>> PublishNewsItem(PublishNewsItemDTO publishNewsItemDTO)
        {
            var picture = await this._mediaProvider.RetrieveMedia(publishNewsItemDTO.MediaFileNames[0]);
            if (picture.SingleOrDefault().Key == false)
            {
                return new Dictionary<bool, string>() { { false, "Problem with retrieving the picture" } };
            }

            try
            {
                var twitterClient = Authenticate();
                this._logger.LogInformation("Uploading file to Twitter.");
                var uploadedImage = await twitterClient.Upload.UploadTweetImageAsync(picture.SingleOrDefault().Value);

                this._logger.LogInformation("Publishing tweet to Twitter, with the uploaded file.");
                await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(publishNewsItemDTO.Summary + "by " + publishNewsItemDTO.Authors[0])
                {
                    Medias = { uploadedImage }
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error with publishing tweet. Message: {message}", exception.Message);
                throw;
            }
            return new Dictionary<bool, string>() { { true, string.Empty } };
        }
    }
}
