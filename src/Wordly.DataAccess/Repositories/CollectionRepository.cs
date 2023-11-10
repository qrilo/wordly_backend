using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wordly.Core.Models.Dtos.Common;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.Connection;
using Wordly.DataAccess.Contracts;
using Wordly.DataAccess.DataManipulation;
using Wordly.DataAccess.Extensions;

namespace Wordly.DataAccess.Repositories;

public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
{
    public CollectionRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<Collection> GetUserCollection(Guid collectionId, Guid userId, bool tracking = false)
    {
        return await Context.Collections
            .WithTracking(tracking)
            .FirstOrDefaultAsync(collection => collection.Id == collectionId && collection.AuthorId == userId);
    }

    public async Task<Collection> GetUserCollectionSummary(Guid collectionId, Guid userId)
    {
        return await Context.Collections
            .AsNoTracking()
            .Include(collection => collection.CollectionTerms)
            .ThenInclude(collectionTerm => collectionTerm.Term)
            .FirstOrDefaultAsync(collection => collection.Id == collectionId && collection.AuthorId == userId);
    }

    public async Task<IReadOnlyCollection<Collection>> GetUserCollections(Guid userId)
    {
        return await Context.Collections
            .AsNoTracking()
            .Where(collection => collection.AuthorId == userId)
            .ToArrayAsync();
    }

    public async Task<IReadOnlyCollection<Collection>> SearchCollections(Guid userId, string name)
    {
        return await Context.Collections
            .AsNoTracking()
            .Include(collection => collection.CollectionTerms)
            .ThenInclude(collectionTerm => collectionTerm.Term)
            .Where(collection => collection.AuthorId == userId 
                                 && EF.Functions.ILike(collection.Name, $"%{name}%")).ToArrayAsync();    
    }
    
    public async Task<Page<Collection>> GetUserCollections(IPageFilter<Collection> pageFilter, Guid userId)
    {
        return await pageFilter.ApplyToQueryable(Context.Collections
            .AsNoTracking()
            .Include(collection => collection.CollectionTerms)
            .ThenInclude(collectionTerm => collectionTerm.Term)
            .Where(collection => collection.AuthorId == userId));
    }
}