using System;
using Azure.Storage.Sas;
using Wordly.Core.Models.Enums;

namespace Wordly.Application.Models.Arguments;

public sealed record GenerateBlobUriArgs(
    BlobContainer Container,
    string BlobName,
    BlobSasPermissions Permissions,
    DateTimeOffset? Lifetime = null);