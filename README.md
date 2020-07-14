# Shrtnr

To get it running:

- Add an appsettings file and fill in the connection string and provide a string to use as the salt for hashids e.g:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "ShrtUrlTableConnectionString": "YOUR_CONNECTION_STRING"
  },
  "AllowedHosts": "*",
  "Salt":  "LITERALLY_ANY_STRING_YOU_LIKE"
}
```

For the ShrtUrlTable I have been using an Azure CosmosDB Table. On Windows it is possible to use the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator) instead of spinning up a resource. I am on mac so have not set it up this way.
