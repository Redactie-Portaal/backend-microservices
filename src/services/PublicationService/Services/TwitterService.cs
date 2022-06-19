using PublicationService.DTOs;
using PublicationService.Interfaces;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace PublicationService.Services
{
    public class TwitterService
    {
        private readonly IMediaProvider _mediaProvider;
        private readonly ILogger _logger;
        private List<IMedia> twitterMedias = new List<IMedia>();

        public TwitterService(IMediaProvider mediaProvider, ILogger<TwitterService> logger)
        {
            this._mediaProvider = mediaProvider;
            this._logger = logger;
        }

        public TwitterClient Authenticate()
        {
            IConfiguration conf = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("twitter_secrets.json").Build();
            var twitterClient = new TwitterClient(conf["Twitter:API_KEY"], conf["Twitter:API_KEY_SECRET"], conf["Twitter:ACCESS_TOKEN"], conf["Twitter:ACCESS_TOKEN_SECRET"]);
            return twitterClient;
        }

        public async Task PublishNewsItem(PublishNewsItemDTO publishNewsItemDTO)
        {
            try
            {
                var twitterClient = Authenticate();
                await PrepareMedia(twitterClient, publishNewsItemDTO);

                string tags = "";
                if (publishNewsItemDTO.Tags.Count != 0)
                {
                    foreach (var tag in publishNewsItemDTO.Tags)
                    {
                        tags += " #" + tag;
                    }
                }

                this._logger.LogInformation("Publishing tweet to Twitter, with the specified file(s).");
                await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(publishNewsItemDTO.Summary + " " + tags)
                {
                    Medias = twitterMedias
                });
                this._logger.LogInformation("Successfully published tweet to Twitter.");
            }
            catch (Exception exception)
            {
                this._logger.LogError("Error with publishing tweet. Message: {message}", exception.Message);
                throw;
            }
        }

        public async Task PrepareMedia(TwitterClient twitterClient, PublishNewsItemDTO publishNewsItemDTO)
        {
            List<byte[]> retrievedPictures = new List<byte[]>();
            List<byte[]> retrievedVideos = new List<byte[]>();

            if (publishNewsItemDTO.Media.Count != 0)
            {
                this._logger.LogInformation("Uploading media to Twitter.");
                foreach (var media in publishNewsItemDTO.Media)
                {
                    var result = await this._mediaProvider.RetrieveMedia(media.FileName);
                    if (result.SingleOrDefault().Key.Contains("image"))
                    {
                        retrievedPictures.Add(result.SingleOrDefault().Value);
                    }
                    if (result.SingleOrDefault().Key.Contains("video"))
                    {
                        retrievedVideos.Add(result.SingleOrDefault().Value);
                    }
                }

                foreach (var picture in retrievedPictures)
                {
                    var uploadedImage = await twitterClient.Upload.UploadTweetImageAsync(picture);
                    twitterMedias.Add(uploadedImage);
                }
                foreach (var video in retrievedVideos)
                {
                    var uploadedVideo = await twitterClient.Upload.UploadTweetVideoAsync(video);
                    twitterMedias.Add(uploadedVideo);
                }
            }
        }
    }
}
