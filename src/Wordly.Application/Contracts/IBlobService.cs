using System.Threading.Tasks;
using Wordly.Application.Models.Arguments;
using Wordly.Core.Models.Enums;

namespace Wordly.Application.Contracts;

public interface IBlobService
{
    Task<string> UploadBlob(UploadBlobArgs args);
    Task DeleteBlob(BlobContainer container, string blobName);
    string GenerateBlobUri(GenerateBlobUriArgs args);
}