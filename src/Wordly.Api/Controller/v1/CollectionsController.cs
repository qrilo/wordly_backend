using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Collection.Test;
using Wordly.Application.Models.Common;
using Wordly.Core.Models.Api;

namespace Wordly.Api.Controller.v1;

[ApiVersion("1.0")]
public class CollectionsController : ApiControllerBase
{
    private readonly ICollectionService _collectionService;
    private readonly ICollectionValidatorsAggregate _collectionValidatorsAggregate;
    public CollectionsController(
        ICollectionService collectionService,
        ICollectionValidatorsAggregate collectionValidatorsAggregate)
    {
        _collectionService = collectionService;
        _collectionValidatorsAggregate = collectionValidatorsAggregate;
    }

    [HttpGet("info")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CollectionInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCollections()
    {
        var result = await _collectionService.GetCollections();

        return Ok(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(IReadOnlyCollection<CollectionInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchCollection([FromQuery] SearchCollectionRequest request)
    {
        var result = await _collectionService.SearchCollection(request);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CollectionInfoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionRequest request)
    {
        await _collectionValidatorsAggregate.CreateCollectionValidator.ValidateAndThrowAsync(request);
        var result = await _collectionService.CreateCollection(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<CollectionInfoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCollections([FromQuery] PagingRequest request)
    {
        var result = await _collectionService.GetCollections(request);

        return Ok(result);
    }

    [HttpGet("{collectionId:guid}")]
    [ProducesResponseType(typeof(CollectionSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetCollection([FromRoute] Guid collectionId)
    {
        var result = await _collectionService.GetCollection(collectionId);

        return Ok(result);
    }

    [HttpPut("{collectionId:guid}")]
    [ProducesResponseType(typeof(CollectionSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCollection([FromRoute] Guid collectionId, [FromBody] UpdateCollectionRequest request)
    {
        await _collectionValidatorsAggregate.UpdateCollectionValidator.ValidateAndThrowAsync(request);
        var result = await _collectionService.UpdateCollection(collectionId, request);

        return Ok(result);
    }

    [HttpDelete("{collectionId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCollection([FromRoute] Guid collectionId)
    {
        await _collectionService.DeleteCollection(collectionId);

        return NoContent();
    }

    [HttpPost("{collectionId:guid}/terms")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTermsToCollection([FromRoute] Guid collectionId, [FromBody] AddTermsToCollectionRequest request)
    {
        await _collectionValidatorsAggregate.AddTermsToCollectionValidator.ValidateAndThrowAsync(request);
        await _collectionService.AddTermsToCollection(collectionId, request);

        return NoContent();
    }

    [HttpPost("{collectionId:guid}/terms/delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTermsFromCollection([FromRoute] Guid collectionId, [FromBody] DeleteTermFromCollectionRequest request)
    {
        await _collectionValidatorsAggregate.DeleteTermFromCollectionValidator.ValidateAndThrowAsync(request);
        await _collectionService.DeleteTermsFromCollection(collectionId, request);

        return NoContent();
    }

    [HttpGet("{collectionId:guid}/test")]
    [ProducesResponseType(typeof(TestResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTest([FromRoute] Guid collectionId, [FromQuery] TestRequest request)
    {
        var result = await _collectionService.GetTest(collectionId, request);
        
        return Ok(result);
    }

    [HttpPost("{collectionId:guid}/test")]
    public async Task<IActionResult> SubmitTest([FromRoute] Guid collectionId, [FromBody] SubmitTestRequest request)
    {
        var result = await _collectionService.SubmitTest(collectionId, request);
        return Ok(result);
    }
    
}