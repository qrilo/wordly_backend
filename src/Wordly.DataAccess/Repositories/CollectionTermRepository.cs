using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.Connection;
using Wordly.DataAccess.Contracts;

namespace Wordly.DataAccess.Repositories;

public class CollectionTermRepository : RepositoryBase<CollectionTerm>, ICollectionTermRepository
{
    public CollectionTermRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyCollection<CollectionTerm>> GetCollectionTerms(Guid collectionId, Guid userId, Guid[] termIds)
    {
        return await Context.CollectionTerms
            .Include(collectionTerm => collectionTerm.Collection)
            .Where(collectionTerm =>
                (collectionTerm.CollectionId == collectionId && collectionTerm.Collection.AuthorId == userId)
                && termIds.Any(id => id == collectionTerm.TermId))
            .ToArrayAsync();
    }
}