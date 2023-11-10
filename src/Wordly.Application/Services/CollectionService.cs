using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Kirpichyov.FriendlyJwt.Contracts;
using Wordly.Application.Contracts;
using Wordly.Application.Extensions;
using Wordly.Application.Mapping;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Collection.Test;
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
        var userId = _tokenReader.GetUserId();
        ;

        var collection = new Collection(request.Name, request.Description, userId);

        _unitOfWork.Collections.Add(collection);
        await _unitOfWork.CommitAsync();

        return _mapper.ToCollectionResponse(collection);
    }

    public async Task<CollectionResponse> UpdateCollection(Guid collectionId, UpdateCollectionRequest request)
    {
        var userId = _tokenReader.GetUserId();
        ;

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
        var userId = _tokenReader.GetUserId();
        ;

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
        var userId = _tokenReader.GetUserId();
        ;

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
        var userId = _tokenReader.GetUserId();
        ;

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

    public async Task<IReadOnlyCollection<TestResponse>> GetTest(Guid collectionId, TestRequest request)
    {
        var userId = _tokenReader.GetUserId();

        var collection = await _unitOfWork.Collections.GetUserCollectionSummary(collectionId, userId);

        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(Collection));
        }

        var terms = collection.CollectionTerms
            .OrderBy(_ => Guid.NewGuid())
            .Select(collectionTerm => collectionTerm.Term)
            .ToList();

        var quantity = request.Quantity > terms.Count ? terms.Count : request.Quantity; 

        var questions = new List<TestResponse>();

        if (request.Single && request.Written && request.Match)
        {
            var takeSingleQuestions = quantity / 3;
            var singleQuestions = GenerateSingleTest(terms, takeSingleQuestions);
            questions.AddRange(singleQuestions);

            var takeWrittenQuestions = quantity / 3;
            var skip = quantity / 3;
            var writtenQuestion = GenerateWrittenTest(terms, takeWrittenQuestions, skip);
            questions.AddRange(writtenQuestion);

            var takeMatchQuestions = terms.Count - (quantity / 3) * 2;
            var skipMatchQuestions = (quantity / 3) * 2;
            var matchQuestion = GenerateMatchTest(terms, takeMatchQuestions, skipMatchQuestions);
            questions.Add(matchQuestion);

            return questions;
        }

        if (request.Single && request.Written)
        {
            var takeSingleQuestions = request.Quantity / 2;
            var singleQuestions = GenerateSingleTest(terms, takeSingleQuestions);
            questions.AddRange(singleQuestions);

            var takeWrittenQuestions = request.Quantity - request.Quantity / 2;
            var skipWrittenQuestion = request.Quantity / 2;
            var writtenQuestion = GenerateWrittenTest(terms, takeWrittenQuestions, skipWrittenQuestion);
            questions.AddRange(writtenQuestion);
            
            return questions;
        }

        if (request.Single && request.Match)
        {   
            var takeSingleQuestions = request.Quantity / 2;
            var singleQuestions = GenerateSingleTest(terms, takeSingleQuestions);
            questions.AddRange(singleQuestions);
            
            var takeMatchQuestions = request.Quantity - request.Quantity / 2;
            var skipMatchQuestions = request.Quantity / 2;
            var matchQuestion = GenerateMatchTest(terms, takeMatchQuestions, skipMatchQuestions);
            questions.Add(matchQuestion);
            
            return questions;
        }

        if (request.Written && request.Match)
        {
            var takeSingleQuestions = request.Quantity / 2;
            var writtenQuestions = GenerateWrittenTest(terms, takeSingleQuestions);
            questions.AddRange(writtenQuestions);
            
            var takeMatchQuestions = request.Quantity - request.Quantity / 2;
            var skipMatchQuestions = request.Quantity / 2;
            var matchQuestion = GenerateMatchTest(terms, takeMatchQuestions, skipMatchQuestions);
            questions.Add(matchQuestion);
            
            return questions;
        }

        if (request.Written && !request.Match && !request.Single)
        {
            var takeSingleQuestions = request.Quantity;
            var writtenQuestions = GenerateWrittenTest(terms, takeSingleQuestions);
            questions.AddRange(writtenQuestions);
            
            return questions;
        }

        if (request.Match && !request.Written && !request.Single)
        {
            var takeMatchQuestions = request.Quantity;
            var matchQuestion = GenerateMatchTest(terms, takeMatchQuestions);
            questions.Add(matchQuestion);
            
            return questions;
        }
        
        var take = request.Quantity;
        var onlySingleQuestions = GenerateSingleTest(terms, take);
        questions.AddRange(onlySingleQuestions);
        
        return questions;
    }

    public async Task<SubmitTestResponse> SubmitTest(Guid collectionId, SubmitTestRequest request)
    {
        var userId = _tokenReader.GetUserId();

        var collection = await _unitOfWork.Collections.GetUserCollectionSummary(collectionId, userId);

        if (collection is null)
        {
            throw new ResourceNotFoundException(nameof(Collection));
        }

        var terms = collection.CollectionTerms.Select(collectionTerm => collectionTerm.Term)
            .ToArray();

        List<UserTerm> wrongAnswerTerms = new();
        
        foreach (var answer in request.Answers)
        {
            var term = terms.FirstOrDefault(term => term.Id == answer.Id);

            if(!String.Equals(term.Term, answer.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                wrongAnswerTerms.Add(term);
            }
        }

        var submitTestResponse = new SubmitTestResponse()
        {
            TotalQuantity = request.Answers.Count,
            TotalKnown = request.Answers.Count - wrongAnswerTerms.Count,
            TotalUnknown = wrongAnswerTerms.Count,
            WrongAnswerTerms = _mapper.MapCollection(wrongAnswerTerms, _mapper.ToTermResponse),
            CorrectAnswersPercent = ((double)(request.Answers.Count - wrongAnswerTerms.Count) / request.Answers.Count) * 100.0,
        };

        return submitTestResponse;
    }

    private IReadOnlyCollection<TestResponse> GenerateSingleTest(List<UserTerm> terms, int take, int skip = 0)
    {
        return terms.Skip(skip).Take(take).Select(question =>
        {
            var answers = terms.Where(term => term.Id != question.Id)
                .OrderBy(term => Guid.NewGuid()).Take(3).Select(term => term.Term).ToList();

            answers.Add(question.Term);

            return new TestResponse()
            {
                QuestionType = QuestionType.Single,
                Single = new SingleResponse()
                {
                    Id = question.Id,
                    Question = question.Definition,
                    Answers = answers.OrderBy(answer => answer).ToArray()
                }
            };
        }).ToArray();
    }

    private IReadOnlyCollection<TestResponse> GenerateWrittenTest(List<UserTerm> terms, int take, int skip = 0)
    {
        return terms.Skip(skip)
            .Take(take)
            .Select(question => new TestResponse()
            {
                QuestionType = QuestionType.Written,
                Written = new WrittenResponse()
                {
                    Id = question.Id,
                    Question = question.Definition
                }
            }).ToArray();
    }

    private TestResponse GenerateMatchTest(List<UserTerm> terms, int take, int skip = 0)
    {
        var matchQuestion = terms.Skip(skip).Take(take).ToArray();

        return new TestResponse()
        {
            QuestionType = QuestionType.Match,
            Match = new MatchResponse()
            {
                Terms = matchQuestion.Select(term => term.Term).ToArray(),
                Definitions = matchQuestion.Select(term => new MatchTermResponse()
                {
                    Id = term.Id,
                    Definition = term.Definition
                }).ToArray(),
            }
        };
    }
}

