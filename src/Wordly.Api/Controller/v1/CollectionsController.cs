using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Collection;
using Wordly.Application.Models.Common;

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
    public async Task<IActionResult> GetCollections()
    {
        var result = await _collectionService.GetCollections();
        
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchCollection([FromQuery] SearchCollectionRequest request)
    {
        var result = await _collectionService.SearchCollection(request);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateCollectionRequest request)
    {
        await _collectionValidatorsAggregate.CreateCollectionValidator.ValidateAndThrowAsync(request);
        var result = await _collectionService.CreateCollection(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetCollections([FromQuery] PagingRequest request)
    {
        var result = await _collectionService.GetCollections(request);

        return Ok(result);
    }

    [HttpGet("{collectionId:guid}")]
    public async Task<IActionResult> GetCollection([FromRoute] Guid collectionId)
    {
        var result = await _collectionService.GetCollection(collectionId);

        return Ok(result);
    }

    [HttpPut("{collectionId:guid}")]
    public async Task<IActionResult> UpdateCollection([FromRoute] Guid collectionId, [FromBody] UpdateCollectionRequest request)
    {
        await _collectionValidatorsAggregate.UpdateCollectionValidator.ValidateAndThrowAsync(request);
        var result = await _collectionService.UpdateCollection(collectionId, request);

        return Ok(result);
    }

    [HttpDelete("{collectionId:guid}")]
    public async Task<IActionResult> DeleteCollection([FromRoute] Guid collectionId)
    {
        await _collectionService.DeleteCollection(collectionId);

        return NoContent();
    }

    [HttpPost("{collectionId:guid}/terms")]
    public async Task<IActionResult> AddTermsToCollection([FromRoute] Guid collectionId, [FromBody] AddTermsToCollectionRequest request)
    {
        await _collectionValidatorsAggregate.AddTermsToCollectionValidator.ValidateAndThrowAsync(request);
        await _collectionService.AddTermsToCollection(collectionId, request);

        return NoContent();
    }

    [HttpPost("{collectionId:guid}/terms/delete")]
    public async Task<IActionResult> DeleteTermsFromCollection([FromRoute] Guid collectionId, [FromBody] DeleteTermFromCollectionRequest request)
    {
        await _collectionValidatorsAggregate.DeleteTermFromCollectionValidator.ValidateAndThrowAsync(request);
        await _collectionService.DeleteTermsFromCollection(collectionId, request);
        
        return NoContent();
    }
}