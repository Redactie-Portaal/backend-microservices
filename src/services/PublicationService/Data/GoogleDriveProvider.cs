using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using PublicationService.Interfaces;

namespace PublicationService.Data
{
    public class GoogleDriveProvider : IMediaProvider
    {
        private readonly ILogger _logger;

        public GoogleDriveProvider(ILogger<GoogleDriveProvider> logger)
        {
            this._logger = logger;
        }

        internal async Task<DriveService> Authenticate()
        {
            UserCredential credential;
            using (var authStream = new FileStream("google_client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStreamAsync(authStream).Result.Secrets,
                    new[] { DriveService.Scope.Drive },
                    "user", CancellationToken.None, new FileDataStore("PublicationGDrive"));
            }

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "UploadTweetDotnetTest"
            });
            return service;
        }

        public async Task<Dictionary<string, byte[]>> RetrieveMedia(string fileName)
        {
            var driveService = await Authenticate();

            try
            {
                var fileSearch = driveService.Files.List();
                fileSearch.Q = $"name = '{fileName}'";

                this._logger.LogInformation("Searching for file in Google Drive.");
                var file = await fileSearch.ExecuteAsync();

                if (file.Files.Count == 0)
                {
                    throw new Exception("Specified file not found in Google Drive.");
                }

                this._logger.LogInformation("Getting file metadata from Google Drive.");
                var fileRequest = driveService.Files.Get(file.Files[0].Id);

                var fileStream = new MemoryStream();
                fileRequest.MediaDownloader.ProgressChanged +=
                    progress =>
                    {
                        switch (progress.Status)
                        {
                            case DownloadStatus.Downloading:
                                {
                                    this._logger.LogInformation("File is downloading from Google Drive. Downloaded bytes thus far: {ByteAmount}", progress.BytesDownloaded);
                                    break;
                                }
                            case DownloadStatus.Completed:
                                {
                                    this._logger.LogInformation("File successfully downloaded from Google Drive.");
                                    break;
                                }
                            case DownloadStatus.Failed:
                                {
                                    this._logger.LogError("Failed to download file from Google Drive.");
                                    break;
                                }
                        }
                    };
                this._logger.LogInformation("Downloading file from Google Drive.");
                fileRequest.Download(fileStream);

                return new Dictionary<string, byte[]>() { { file.Files[0].MimeType, fileStream.ToArray() } };
            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with retrieve the file from Google Drive. Error message: {Message}", ex.Message);
                throw;
            }
        }
    }
}
