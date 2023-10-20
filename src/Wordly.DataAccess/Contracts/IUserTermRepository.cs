using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Core.Models.Dtos.Common;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.DataManipulation;

namespace Wordly.DataAccess.Contracts;

public interface IUserTermRepository : IRepositoryBase<UserTerm>
{
    Task<IReadOnlyCollection<UserTerm>> GetForUser(Guid userId);
    Task<UserTerm> GetForUser(Guid userId, Guid userTermId);
    Task<IReadOnlyCollection<UserTerm>> GetForUser(Guid userId, Guid[] ids);
    Task<Page<UserTerm>> GetForUser(IPageFilter<UserTerm> pageFilter, Guid userId);
    Task<IReadOnlyCollection<UserTerm>> GetNotAddedToCollection(Guid userId, Guid[] ids, Guid[] existingIds);
}