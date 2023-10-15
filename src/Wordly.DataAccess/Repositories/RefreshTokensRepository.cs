using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.Connection;
using Wordly.DataAccess.Contracts;
using Wordly.DataAccess.Extensions;

namespace Wordly.DataAccess.Repositories;

public sealed class RefreshTokensRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokensRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<RefreshToken> FindById(Guid id, bool useTracking)
    {
        return await Context.RefreshTokens
            .WithTracking(useTracking).FirstOrDefaultAsync(token => token.Id == id);
    }

    public async Task<IReadOnlyCollection<RefreshToken>> FindAllByUserId(Guid userId, bool useTracking)
    {
        return await Context.RefreshTokens
            .WithTracking(useTracking)
            .Where(token => token.UserId == userId)
            .ToArrayAsync();
    }
}