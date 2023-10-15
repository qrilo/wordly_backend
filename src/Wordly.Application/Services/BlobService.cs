using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Arguments;
using Wordly.Core.Models.Enums;
using Wordly.Core.Options;

namespace Wordly.Application.Services;

public sealed class BlobService : IBlobService
{
    private static readonly DateTimeOffset MaxSasExpiresOn = new(new DateTime(9999, 1, 1));
    private readonly BlobOptions _blobOptions;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobService(IOptions<BlobOptions> blobOptions)
    {
        _blobOptions = blobOptions.Value;
        _blobServiceClient = new BlobServiceClient(_blobOptions.ConnectionString);
    }

    public async Task<string> UploadBlob(UploadBlobArgs args)
    {
        var blobId = Guid.NewGuid();

        var blobName = args.BlobFolderPrefix is null
            ? blobId.ToString()
            : Path.Combine(args.BlobFolderPrefix, blobId.ToString());

        var blobClient = GetBlobClient(args.Container, blobName);

        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = args.ContentType,
            ContentDisposition = args.GetContentDisposition(),
        };

        await blobClient.UploadAsync(args.Blob.Value, blobHttpHeader);
        return blobClient.Name;
    }

    public async Task DeleteBlob(BlobContainer container, string blobName)
    {
        var blobClient = GetBlobClient(container, blobName);
        await blobClient.DeleteAsync();
    }

    public string GenerateBlobUri(GenerateBlobUriArgs args)
    {
        var blobClient = GetBlobClient(args.Container, args.BlobName);
        return blobClient.GenerateSasUri(args.Permissions, args.Lifetime ?? MaxSasExpiresOn).ToString();
    }

    private BlobClient GetBlobClient(BlobContainer container, string blobName)
    {
        var containerName = GetContainerName(container);
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        return containerClient.GetBlobClient(blobName);
    }

    private string GetContainerName(BlobContainer container)
    {
        return _blobOptions.Containers.FirstOrDefault(configuration => configuration.Name == container)?.Path
               ?? throw new ArgumentException($"Path for container '{container}' is not provided", nameof(container));
    }
}