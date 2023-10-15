﻿using System.Collections.Generic;
using Wordly.Core.Models.Dtos.Common;

namespace Wordly.Application.Models.Common;

public sealed class PagingResponse<TItem> : ItemsContainerModel<TItem>
{
    public int TotalPagesCount { get; init; }
    public int CurrentPage { get; init; }

    public static PagingResponse<TItem> CreateFromPage<TSourceItem>(
        Page<TSourceItem> page,
        IReadOnlyCollection<TItem> items)
    {
        return new PagingResponse<TItem>()
        {
            Items = items,
            ItemsTotalCount = page.ItemsCount,
            CurrentPage = page.PageNumber,
            TotalPagesCount = page.PageCount,
        };
    }
}