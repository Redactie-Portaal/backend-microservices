using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.StaticFiles;
using NewsItemService.Entities;
using NewsItemService.Interfaces;

namespace NewsItemService.Data
{
    public class MediaRepository : IMediaRepository
    {
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

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "OFMicroservicesDotnet"
            });
            return service;
        }

        public async Task<Dictionary<bool, Media>> GetMediaByFilename(string fileName)
        {
            var driveService = await Authenticate();

            // Retrieve file information by name
            var fileSearch = driveService.Files.List();
            fileSearch.Q = $"name = {fileName}";
            var file = await fileSearch.ExecuteAsync();

            if (file.Files.Count == 0)
            {
                return new Dictionary<bool, Media>() { { false, null } };
            }

            // Get specific file data by Drive Id
            var fileRequest = driveService.Files.Get(file.Files[0].Id);

            var fileStream = new MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            fileRequest.MediaDownloader.ProgressChanged +=
                progress =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            fileRequest.Download(fileStream);

            var media = new Media();

            //TODO: use the DTO instead of the Entity
            //media.Id = file.Files[0].Id;
            media.FileName = file.Files[0].Name;
            media.Source = "Google Drive";

            var contentType = file.Files[0].MimeType;
            if (contentType.Contains("application"))
            {
                media.NewsItemMediaType = Types.NewsItemMediaType.Document;
            }
            else if (contentType.Contains("audio"))
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

        public async Task<Dictionary<bool, string>> SaveMedia(Media media)
        {
            var driveService = await Authenticate();

            var file = new Google.Apis.Drive.v3.Data.File();
            file.Name = media.FileName;

            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(media.FileName, out var contentType);
            file.MimeType = contentType;

            var fileUpload = driveService.Files.Create(file).ExecuteAsync();

            if (fileUpload.IsCompletedSuccessfully)
            {
                return new Dictionary<bool, string>() { { true, string.Empty } };
            }
            else
            {
                return new Dictionary<bool, string>() { { false, "File could not be saved" } };
            }
        }
    }
}
