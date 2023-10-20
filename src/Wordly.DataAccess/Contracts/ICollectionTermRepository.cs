using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Core.Models.Dtos.Common;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.DataManipulation;

namespace Wordly.DataAccess.Contracts;

public interface ICollectionTermRepository : IRepositoryBase<CollectionTerm>
{
    Task<IReadOnlyCollection<CollectionTerm>> GetCollectionTerms(Guid collectionId, Guid userId, Guid[] termIds);
}