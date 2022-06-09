# NewsItemService

## Overview
* This microservices is responsible for retrieving, creating and updating a NewsItem, that can be published to various publication like Twitter and more...

## Setup - database
* This microservice uses a PostgreSQL as its database for saving the data of a newsitem.
* In order for the service to access the database, a connectionstring needs to be present in the user's Environment variable that has a name of `redactieportaal_db_string`.
* A template of the connectionstring is can be found below, make sure to replace the values with your own database configuration if needed:
```
"Server=localhost;Port=5432;Database=redactieportaal;UserId=developer;Password=Flevoland"
```
* The service creates the tables in the database upon starting the application.

# Setup - file storage
* This microservice uses the Google Drive API for retrieving and saving media that belongs to a newsitem.
* In order to use the API with the application, read the section about Google Drive API in the 'tranfer document'.
* To allow the service to communicate with the API, place a json file, with the OAuth credentials, inside the folder of the NewsItemService. This file needs to have the following name: `google_client_secret.json`.
* THE JSON FILE CANNOT, UNDER ANY CIRCUMSTANCES, BE CHECKED INTO VERSION CONTROL.

## Setup - RabbitMQ
* TO BE FILLED IN ...