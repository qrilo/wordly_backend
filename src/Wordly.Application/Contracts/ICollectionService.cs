using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Collection.Test;
using Wordly.Application.Models.Common;

namespace Wordly.Application.Contracts;

public interface ICollectionService
{
    Task<IReadOnlyCollection<CollectionInfoResponse>> GetCollections();
    Task<PagingResponse<CollectionResponse>> GetCollections(PagingRequest request);
    Task<CollectionSummaryResponse> GetCollection(Guid collectionId);
    Task<CollectionResponse> CreateCollection(CreateCollectionRequest request);
    Task<CollectionResponse> UpdateCollection(Guid collectionId, UpdateCollectionRequest request);
    Task DeleteCollection(Guid collectionId);
    Task AddTermsToCollection(Guid collectionId, AddTermsToCollectionRequest request);
    Task DeleteTermsFromCollection(Guid collectionId, DeleteTermFromCollectionRequest request);
    Task<IReadOnlyCollection<CollectionResponse>> SearchCollection(SearchCollectionRequest request);
    Task<IReadOnlyCollection<TestResponse>> GetTest(Guid collectionId, TestRequest request);
    Task<SubmitTestResponse> SubmitTest(Guid collectionId, SubmitTestRequest request);
}