using Wordly.Core.Models.Enums;

namespace Wordly.Core.Options;

public sealed class BlobOptions
{
    public string ConnectionString { get; set; }
    public BlobContainerConfiguration[] Containers { get; set; }
    public long MaxChatMessageFileSizeBytes { get; set; }
    
    public sealed class BlobContainerConfiguration
    {
        public BlobContainer Name { get; set; }
        public string Path { get; set; }
    }
}