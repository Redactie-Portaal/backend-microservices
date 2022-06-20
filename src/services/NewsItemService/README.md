# NewsItemService

## Overview
* This microservices is responsible for retrieving, creating and updating a NewsItem, that can be published to various publication like Twitter and more...

## Setup - database
* This microservice uses a PostgreSQL as its database for saving the data of a newsitem.
* In order for the service to access the database, a connectionstring needs to be present in the user's Environment variable that has a name of `redactieportaal_db_string`.
* A template of the connectionstring is can be found below, make sure to replace the values with your own database configuration if needed:
```
"Server=localhost;Port=5432;Database=DATABASE_NAME;UserId=developer;Password=Flevoland"
```
* The service creates the tables in the database upon starting the application.
* A new database will be created with the name of the microservice.

# Setup - file storage
* This microservice uses the Google Drive API for retrieving and saving media that belongs to a newsitem.
* In order to use the API with the application, read the section about Google Drive API in the 'transfer document'.
* To allow the service to communicate with the API, place a json file, with the OAuth credentials, inside the folder of the NewsItemService. This file needs to have the following name: `google_client_secret.json`.
* After placing the file, right click on it in Visual Studio and select 'Properties'. Under 'Copy to Output Directory', set it to 'Copy if newer'.
* THE JSON FILE CANNOT, UNDER ANY CIRCUMSTANCES, BE CHECKED INTO VERSION CONTROL.

## Setup - RabbitMQ
* This microservice uses RabbitMQ for inter communication with different microservices.
* In order for the service to access the database, a RabbitMQ password needs to be present in the user's Environment variable that has a name of `redactieportaal_rabbitmq_pass`.
* The default password is 'guest'.