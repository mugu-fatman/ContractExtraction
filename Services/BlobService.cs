using Azure.Storage.Blobs;

namespace Frame.ContractExtraction.API.Services;

public class BlobService
{
    private readonly BlobContainerClient _container;

    public BlobService(IConfiguration config)
    {
        var connectionString = config["Blob:ConnectionString"];
        var containerName = config["Blob:ContainerName"];
        _container = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<byte[]> DownloadAsync(string blobName)
    {
        var blobClient = _container.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
            throw new Exception("Blob not found");

        var download = await blobClient.DownloadContentAsync();
        return download.Value.Content.ToArray();
    }
}