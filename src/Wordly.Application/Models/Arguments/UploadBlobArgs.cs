using System;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Unidecode.NET;
using Wordly.Core.Models.Enums;

namespace Wordly.Application.Models.Arguments;

public sealed class UploadBlobArgs
{
    public UploadBlobArgs(
        Stream blob,
        BlobContainer container,
        string contentType,
        string filename,
        bool isInline = false,
        string blobFolderPrefix = null)
    {
        Blob = new Lazy<Stream>(() => blob);
        Container = container;
        ContentType = contentType;
        Filename = filename;
        IsInline = isInline;
        BlobFolderPrefix = blobFolderPrefix;
    }
    
    public UploadBlobArgs(
        IFormFile file,
        BlobContainer container,
        bool isInline = false,
        string blobFolderPrefix = null)
    {
        Blob = new Lazy<Stream>(file.OpenReadStream);
        Container = container;
        ContentType = file.ContentType;
        Filename = file.FileName;
        IsInline = isInline;
        BlobFolderPrefix = blobFolderPrefix;
    }

    public Lazy<Stream> Blob { get; }
    public BlobContainer Container { get; }
    public string ContentType { get; }
    public string Filename { get; }
    public string BlobFolderPrefix { get; }
    public bool IsInline { get; }

    public string GetContentDisposition()
    {
        var type = IsInline ? "inline" : "attachment";
        var filename = Filename.Unidecode();
        
        // https://www.edureka.co/community/41331/friendly-filename-when-downloading-azure-blob
        var filenameUtf8 = HttpUtility.UrlEncode(Filename, System.Text.Encoding.UTF8)?.Replace("+", " ");

        return $"{type}; filename=\"{filename}\"; filename*=UTF-8''{filenameUtf8}";
    }
}