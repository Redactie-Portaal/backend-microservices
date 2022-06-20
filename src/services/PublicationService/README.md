
# PublicationService

* Publishing a NewsItem to a specific Publication like Twitter, site, etc.

## Publication Restrictions

### Twitter

* A Tweet can only contain 4 pictures, each being max 5MB in size. 
* The supported formats for pictures are: JPG, PNG, GIF & WEBP.

* A Tweet can only contain 1 video, being max 15MB in size.
* The supported format for a video is MP4 (H264).
* The video resolution are: 1280x720 (landscape), 720x1280 (portrait) & 720x720 (square).

## Technical debt

* A Tweet cannot contain a video and pictures. There is no logic that checks for this but can be added if
time allows it.

# Setup - file storage

* This microservice uses the Google Drive API for retrieving media that belongs to a newsitem.
* In order to use the API with the application, read the section about Google Drive API in the 'transfer document'.
* To allow the service to communicate with the API, place a json file, with the OAuth credentials, inside the folder of the NewsItemService. This file needs to have the following name: `google_client_secret.json`.
* After placing the file, right click on it in Visual Studio and select 'Properties'. Under 'Copy to Output Directory', set it to 'Copy if newer'.
* THE JSON FILE CANNOT, UNDER ANY CIRCUMSTANCES, BE CHECKED INTO VERSION CONTROL.