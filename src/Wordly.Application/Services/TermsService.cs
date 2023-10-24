using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Azure.Storage.Sas;
using Kirpichyov.FriendlyJwt.Contracts;
using Wordly.Application.Contracts;
using Wordly.Application.Extensions;
using Wordly.Application.Mapping;
using Wordly.Application.Models.Arguments;
using Wordly.Application.Models.Common;
using Wordly.Application.Models.Terms;
using Wordly.Core.Exceptions;
using Wordly.Core.Models.Entities;
using Wordly.Core.Models.Enums;
using Wordly.DataAccess.Contracts;
using Wordly.DataAccess.DataManipulation;

namespace Wordly.Application.Services;

public class TermsService : ITermsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectsMapper _mapper;
    private readonly IJwtTokenReader _tokenReader;
    private readonly IBlobService _blobService;
    public TermsService(
        IUnitOfWork unitOfWork,
        IJwtTokenReader tokenReader,
        IObjectsMapper mapper,
        IBlobService blobService)
    {
        _unitOfWork = unitOfWork;
        _tokenReader = tokenReader;
        _mapper = mapper;
        _blobService = blobService;
    }

    public async Task<IReadOnlyCollection<TermResponse>> GetTerms()
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var userTerms = await _unitOfWork.UserTerms.GetForUser(userId);

        return _mapper.MapCollection(userTerms, _mapper.ToTermResponse);
    }

    public async Task<TermCreatedResponse> CreateTerm(CreateTermRequest request)
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var term = new UserTerm(request.Term, request.Definition, userId, request.Tags, request.Description);

        if (request.Image is not null && request.Image.Length > 0)
        {
            var uploadBlobArgs = new UploadBlobArgs(request.Image, BlobContainer.TermImages, isInline: true);
            var blobName = await _blobService.UploadBlob(uploadBlobArgs);
            var blobId = Guid.Parse(blobName);

            var generateUriArgs = new GenerateBlobUriArgs(BlobContainer.TermImages, blobName, BlobSasPermissions.Read);
            var blobUri = _blobService.GenerateBlobUri(generateUriArgs);

            term.SetImageUrlAndBlobId(blobUri, blobId);
        }

        _unitOfWork.UserTerms.Add(term);
        await _unitOfWork.CommitAsync();

        return _mapper.ToTermCreatedResponse(term);
    }

    public async Task DeleteTerms(DeleteTermsRequest request)
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var userTerms = await _unitOfWork.UserTerms.GetForUser(userId, request.Ids);

        var tasks = userTerms
            .Where(term => term.ImageBlobId is not null)
            .Select(term =>
            _blobService.DeleteBlob(BlobContainer.TermImages, term.ImageBlobId.ToString())
        );

        _unitOfWork.UserTerms.RemoveRange(userTerms);
        await _unitOfWork.CommitAsync();

        await Task.WhenAll(tasks);
    }

    public async Task<TermUpdatedResponse> UpdateTerm(Guid userTermId, UpdateTermRequest request)
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var userTerm = await _unitOfWork.UserTerms.GetForUser(userId, userTermId);

        if (userTerm is null)
        {
            throw new ResourceNotFoundException(nameof(UserTerm));
        }

        if (request.Image is not null && request.Image.Length > 0)
        {
            if (userTerm.ImageBlobId is not null)
            {
                await _blobService.DeleteBlob(BlobContainer.TermImages, userTerm.ImageBlobId.ToString());
            }

            var uploadBlobArgs = new UploadBlobArgs(request.Image, BlobContainer.TermImages, isInline: true);
            var blobName = await _blobService.UploadBlob(uploadBlobArgs);
            var blobId = Guid.Parse(blobName);

            var generateUriArgs = new GenerateBlobUriArgs(BlobContainer.TermImages, blobName, BlobSasPermissions.Read);
            var blobUri = _blobService.GenerateBlobUri(generateUriArgs);

            userTerm.SetImageUrlAndBlobId(blobUri, blobId);
        }

        userTerm.SetTerm(request.Term);
        userTerm.SetDefinition(request.Definition);
        userTerm.SetTags(request.Tags);
        userTerm.SetDescription(request.Description);

        await _unitOfWork.CommitAsync();

        return _mapper.ToTermUpdateResponse(userTerm);
    }

    public async Task<PagingResponse<TermResponse>> GetTerms(TermPagingRequest request)
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var pageFilter = new PageFilter<UserTerm>()
        {
            PageNumber = request.Page,
            PageSize = request.PageSize,
            OrderingExpression = request.SortBy == SortBy.Term ? userTerm => userTerm.Term : userTerm => userTerm.CreatedAtUtc,
            OrderingDirection = SortDirection.Asc == request.SortDirection ? OrderingDirection.Ascending : OrderingDirection.Descending,
            FilteringExpression = BuildFilterExpression(request)
        };

        var page = await _unitOfWork.UserTerms.GetForUser(pageFilter, userId);
        var models = _mapper.MapCollection(page.Items, _mapper.ToTermResponse);

        return PagingResponse<TermResponse>.CreateFromPage(page, models);
    }
    
    public Expression<Func<UserTerm, bool>> BuildFilterExpression(TermPagingRequest request)
    {
        var expression = PredicateBuilder.True<UserTerm>();

        if (request.SearchPhrase is not null && request.SearchPhrase.Length > 0)
        {
            if (request.SearchIn is SearchIn.Term)
            {
                expression = expression.And(userTerm => userTerm.Term.ToLower().Contains(request.SearchPhrase.ToLower()));
            }
            else if (request.SearchIn is SearchIn.Definition)
            {
                expression = expression.And(userTerm => userTerm.Definition.ToLower().Contains(request.SearchPhrase.ToLower()));
            }
            else if (request.SearchIn is SearchIn.All)
            {
                expression = expression.And(userTerm => userTerm.Term.ToLower().Contains(request.SearchPhrase.ToLower()) 
                                                        || userTerm.Definition.ToLower().Contains(request.SearchPhrase.ToLower()));
            }
        }

        /*if (request.Tags is not null && request.Tags.Length > 0)
        {
            var tagsNormalized = request.Tags.Select(tag => tag.ToLowerInvariant()).ToArray();

            expression = expression.And(x => tagsNormalized.Contains(x.TagsRaw));
        }*/

        return expression;
    }

    public async Task DeleteTermImage(Guid termId)
    {
        var userId = Guid.Parse(_tokenReader.UserId);

        var userTerm = await _unitOfWork.UserTerms.GetForUser(userId, termId);

        if (userTerm is null)
        {
            throw new ResourceNotFoundException(nameof(UserTerm));
        }

        if (!userTerm.ImageBlobId.HasValue)
        {
            return;
        }

        await _blobService.DeleteBlob(BlobContainer.TermImages, userTerm.ImageBlobId.ToString());
        userTerm.DeleteImage();

        await _unitOfWork.CommitAsync();
    }
}