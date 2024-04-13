# Installation of MongoDB

Follow the steps below to install MongoDB:

## Windows

1. Download the MongoDB installer file from the MongoDB website.

2. Open the downloaded file to start the MongoDB installation wizard.

3. Follow the installation wizard to install MongoDB.

4. To ensure MongoDB was installed correctly, open the Command Prompt and execute the following command:

```shell
mongo --version
```

## Using MongoDB Compass to Create a Database

MongoDB Compass is a GUI application that allows you to interact with MongoDB. Here's how you can create a database using MongoDB Compass:

1. Download and install MongoDB Compass from the MongoDB website.

2. Open MongoDB Compass.

3. Connect to your MongoDB server by entering your connection string in the connection field. If you're running MongoDB on your local
   machine, you can use the default connection string `mongodb://localhost:27017`.

4. Once connected, you'll see a list of your databases on the left side of the screen. To create a new database, click on the "CREATE
   DATABASE" button near the top of the screen.

5. In the "Create Database" dialog box, enter the name of your new database in the "Database Name" field.

6. MongoDB requires you to create at least one collection when creating a database. Enter the name of your new collection in the "Collection
   Name" field.

7. Click on the "Create Database" button to create your new database and collection.

If MongoDB is installed correctly, the version of MongoDB will be displayed.

# Configuration of `appsettings.json`

The `appsettings.json` file in your project should be configured as follows:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "MongoDbConnectionString": "mongodb://localhost:27017",
  "MongoDbDatabase": "your_database_name",
  "WatchPath": "your_watch_path",
  "ArchivePath": "your_archive_path",
  "TempPath": "your_temp_path"
}
```

Replace `your_database_name`, `your_watch_path`, `your_archive_path`, and `your_temp_path` with your actual MongoDB database name, watch
path, archive path, and temporary path respectively.

The `MongoDbConnectionString` is the connection string to your MongoDB server. In this case, it is running on the same machine as the
application (localhost) on the default MongoDB port (27017).

The `WatchPath` is the directory that the application will monitor for new CSV files to import.

The `ArchivePath` is the directory where the application will move CSV files after they have been imported.

The `TempPath` is a temporary directory that the application uses when importing CSV files.