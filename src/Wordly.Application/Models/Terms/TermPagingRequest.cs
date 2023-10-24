using System;
using Wordly.Application.Models.Common;

namespace Wordly.Application;

public sealed class TermPagingRequest : PagingRequest
{
    public SortBy SortBy { get; init; } = SortBy.CreatedAtUtc;
    public SortDirection SortDirection { get; init; } = SortDirection.Desc;
    public string SearchPhrase { get; init; }
    public SearchIn SearchIn { get; init; } = SearchIn.Term;
    public string[] Tags { get; init; }
}

public enum SearchIn
{
    Term,
    Definition,
    All
}

public enum SortBy
{
    Term,
    CreatedAtUtc
}

public enum SortDirection
{
    Asc,
    Desc
}