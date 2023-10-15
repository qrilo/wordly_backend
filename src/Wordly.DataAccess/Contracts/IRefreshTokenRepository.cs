using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Core.Models.Entities;

namespace Wordly.DataAccess.Contracts;

public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
    Task<RefreshToken> FindById(Guid id, bool useTracking);
    Task<IReadOnlyCollection<RefreshToken>> FindAllByUserId(Guid userId, bool useTracking);
}