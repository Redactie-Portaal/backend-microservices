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
                ApplicationName = "UploadTweetDotnetTest"
            });
            return service;
        }

        public async Task<Dictionary<bool, byte[]>> RetrieveMedia(string fileName)
        {
            var driveService = await Authenticate();

            // Retrieve file information by name
            var fileSearch = driveService.Files.List();
            fileSearch.Q = $"name = {fileName}";
            var file = await fileSearch.ExecuteAsync();

            if (file.Files.Count == 0)
            {
                return new Dictionary<bool, byte[]>() { { false, null} };
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
            return new Dictionary<bool, byte[]>() { { true, fileStream.ToArray() } };
        }
    }
}
