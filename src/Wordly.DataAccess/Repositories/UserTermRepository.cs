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

namespace Wordly.DataAccess.Repositories;

public class UserTermRepository : RepositoryBase<UserTerm>, IUserTermRepository
{
    public UserTermRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyCollection<UserTerm>> GetForUser(Guid userId)
    {
        return await Context.UserTerms.AsNoTracking()
            .Where(userTerm => userTerm.UserId == userId).ToArrayAsync();
    }

    public async Task<UserTerm> GetForUser(Guid userId, Guid userTermId)
    {
        return await Context.UserTerms.FirstOrDefaultAsync(userTerm => userTerm.Id == userTermId && userTerm.UserId == userId);
    }

    public async Task<IReadOnlyCollection<UserTerm>> GetForUser(Guid userId, Guid[] ids)
    {
        return await Context.UserTerms.Where(userTerm => ids.Contains(userTerm.Id) && userTerm.UserId == userId)
            .ToArrayAsync();
    }
    
    public async Task<IReadOnlyCollection<UserTerm>> GetNotAddedToCollection(Guid userId, Guid[] ids, Guid[] existingIds)
    {
        return await Context.UserTerms
            .Where(userTerm => ids.Contains(userTerm.Id) && userTerm.UserId == userId && !existingIds.Contains(userTerm.Id))
            .ToArrayAsync();
    }

    public async Task<Page<UserTerm>> GetForUser(IPageFilter<UserTerm> pageFilter, Guid userId)
    {
        return await pageFilter.ApplyToQueryable(Context.UserTerms.AsNoTracking()
            .Where(userTerm => userTerm.UserId == userId));
    }
}