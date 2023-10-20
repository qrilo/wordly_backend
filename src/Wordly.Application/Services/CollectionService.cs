using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kirpichyov.FriendlyJwt.Contracts;
using Wordly.Application.Contracts;
using Wordly.Application.Extensions;
using Wordly.Application.Mapping;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Common;
using Wordly.Core.Exceptions;
using Wordly.Core.Models.Entities;
using Wordly.DataAccess.Contracts;
using Wordly.DataAccess.DataManipulation;

namespace Wordly.Application.Services;

public class CollectionService : ICollectionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectsMapper _mapper;
    private readonly IJwtTokenReader _tokenReader;
    public CollectionService(
        IUnitOfWork unitOfWork,
        IObjectsMapper mapper,
        IJwtTokenReader tokenReader)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _tokenReader = tokenReader;
    }

    public async Task<IReadOnlyCollection<CollectionInfoResponse>> GetCollections()
    {
        var userId = _tokenReader.GetUserId();

        var collections = await _unitOfWork.Collections.GetUserCollections(userId);

        return _mapper.MapCollection(collections, _mapper.ToCollectionInfoResponse);
    }

    public async Task<PagingResponse<CollectionResponse>> GetCollections(PagingRequest request)
    {
        var userId = _tokenReader.GetUserId();
        
        var pageFilter = new PageFilter<Collection>()
        {
            PageNumber = request.Page,
            PageSize = request.PageSize,
            OrderingExpression = collection => collection.CreatedAtUtc,
            OrderingDirection = OrderingDirection.Descending,
        };

        var page = await _unitOfWork.Collections.GetUserCollections(pageFilter, userId);
        var models = _mapper.MapCollection(page.Items, _mapper.ToCollectionResponse);
        
        return PagingResponse<CollectionResponse>.CreateFromPage(page, models);
    }

    public async Task<CollectionSummaryResponse> GetCollection(Guid collectionId)
    {
        var userId = _tokenReader.GetUserId();

        var collection = await _unitOfWork.Collections.GetUserCollectionSummary(collectionId, userId);
        
        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(Collection));
        }

        var terms = collection.CollectionTerms.Select(collectionTerm => collectionTerm.Term).ToArray();
        
        return _mapper.ToCollectionSummaryResponse(collection, terms);
    }

    public async Task<CollectionResponse> CreateCollection(CreateCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();;
        
        var collection = new Collection(request.Name, request.Description, userId);
        
        _unitOfWork.Collections.Add(collection);
        await _unitOfWork.CommitAsync();

        return _mapper.ToCollectionResponse(collection);
    }

    public async Task<CollectionResponse> UpdateCollection(Guid collectionId, UpdateCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();;

        var collection = await _unitOfWork.Collections.GetUserCollection(collectionId, userId, true);

        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(collection));
        }

        collection.SetName(request.Name);
        collection.SetDescription(request.Description);

        await _unitOfWork.CommitAsync();

        return _mapper.ToCollectionResponse(collection);
    }

    public async Task DeleteCollection(Guid collectionId)
    {
        var userId = _tokenReader.GetUserId();;

        var collection = await _unitOfWork.Collections.GetUserCollection(collectionId, userId);

        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(Collection));
        }
        
        _unitOfWork.Collections.Remove(collection);
        await _unitOfWork.CommitAsync();
    }

    public async Task AddTermsToCollection(Guid collectionId, AddTermsToCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();;

        var collection = await _unitOfWork.Collections.GetUserCollectionSummary(collectionId, userId);

        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(collection));
        }

        var termIds = collection.CollectionTerms
            .Select(term => term.TermId);

        var termIdsIsNotCollection = request.Ids.Except(termIds);
        
        var collectionTerms = termIdsIsNotCollection.Select(term => new CollectionTerm(term, collection.Id));
        
        _unitOfWork.CollectionTerms.AddRange(collectionTerms);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteTermsFromCollection(Guid collectionId, DeleteTermFromCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();;

        var collectionTerms = await _unitOfWork.CollectionTerms.GetCollectionTerms(collectionId, userId, request.Ids);
        
        _unitOfWork.CollectionTerms.RemoveRange(collectionTerms);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IReadOnlyCollection<CollectionResponse>> SearchCollection(SearchCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();
        
        var collections = await _unitOfWork.Collections.SearchCollections(userId, request.Name);    
        
        return _mapper.MapCollection(collections, _mapper.ToCollectionResponse);
    }
}