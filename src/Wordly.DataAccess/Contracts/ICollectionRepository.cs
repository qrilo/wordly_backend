using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Core.Models.Dtos.Common;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.DataManipulation;

namespace Wordly.DataAccess.Contracts;

public interface ICollectionRepository : IRepositoryBase<Collection>
{
    Task<Collection> GetUserCollection(Guid collectionId, Guid userId, bool tracking = false);
    Task<Page<Collection>> GetUserCollections(IPageFilter<Collection> pageFilter, Guid userId);
    Task<Collection> GetUserCollectionSummary(Guid collectionId, Guid userId);
    Task<IReadOnlyCollection<Collection>> GetUserCollections(Guid userId);
    Task<IReadOnlyCollection<Collection>> SearchCollections(Guid userId, string name);
}