namespace Wordly.Application.Models.Common;

public class PagingRequest
{
    public const int MaxPageSize = 50;

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = MaxPageSize;
}