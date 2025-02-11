using Azure.Storage.Blobs;

public class FileService
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public FileService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlobStorage:ConnectionString"];
        _containerName = configuration["AzureBlobStorage:ContainerName"];
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var blobContainerClient = new BlobContainerClient(_connectionString, _containerName);

        await blobContainerClient.CreateIfNotExistsAsync();

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        var blobClient = blobContainerClient.GetBlobClient(fileName);

        using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream);
        }

        return blobClient.Uri.ToString();
    }
}