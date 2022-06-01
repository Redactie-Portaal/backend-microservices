using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.StaticFiles;
using NewsItemService.Entities;
using NewsItemService.Interfaces;
using System.Text;

namespace NewsItemService.Data
{
    public class MediaRepository : IMediaRepository
    {
        private readonly ILogger _logger;
        public MediaRepository(ILogger<MediaRepository> logger)
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
                    "user", CancellationToken.None, new FileDataStore("Drive.ListDrive"));
            }

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "OFFRedactieportaalMicroservices"
            });
            return service;
        }

        public async Task<Dictionary<bool, Media>> GetMediaByFilename(string fileName)
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
                    return new Dictionary<bool, Media>() { { false, null } };
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

                var media = new Media();

                media.Id = file.Files[0].Id;
                media.FileName = file.Files[0].Name;

                var contentType = file.Files[0].MimeType;
                if (contentType.Contains("audio"))
                {
                    media.NewsItemMediaType = Types.NewsItemMediaType.Audio;
                }
                else if (contentType.Contains("image"))
                {
                    media.NewsItemMediaType = Types.NewsItemMediaType.Picture;
                }
                else if (contentType.Contains("video"))
                {
                    media.NewsItemMediaType = Types.NewsItemMediaType.Video;
                }
                else
                {
                    media.NewsItemMediaType = Types.NewsItemMediaType.Document;
                }
                return new Dictionary<bool, Media>() { { true, media } };
            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with retrieve the file from Google Drive. Error message: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<Dictionary<bool, string>> SaveMedia(Media media)
        {
            var driveService = await Authenticate();

            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = media.FileName;

            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(media.FileName, out var contentType);
            file.MimeType = contentType;

            MemoryStream fileStream = new MemoryStream(media.FileContent);

            try
            {
                var fileUpload = driveService.Files.Create(file, fileStream, file.MimeType);

                this._logger.LogInformation("Uploading file to Google Drive.");
                var result = await fileUpload.UploadAsync();

                if (result.Status == Google.Apis.Upload.UploadStatus.Failed)
                {
                    this._logger.LogError("The file upload to Google Drive failed. Error message: {Message}", result.Exception);
                    throw result.Exception;
                }
                this._logger.LogInformation("File upload to Google Drive succeeded.");
                return new Dictionary<bool, string>() { { true, fileUpload.ResponseBody.Id } };

            }
            catch (Exception ex)
            {
                this._logger.LogError("There is a problem with uploading the file to Google Drive. Error message: {Message}", ex.Message);
                throw;
            }
        }
    }
}
