using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wordly.Application;
using Wordly.Application.Contracts;
using Wordly.Application.Models.Common;
using Wordly.Application.Models.Terms;
using Wordly.Core.Models.Api;

namespace Wordly.Api.Controller.v1;

[ApiVersion("1.0")]
public class TermsController : ApiControllerBase
{
    private readonly ITermsService _termsService;
    private readonly ITermValidatorsAggregate _termValidatorsAggregate;
    public TermsController(
        ITermsService termsService,
        ITermValidatorsAggregate termValidatorsAggregate)
    {
        _termsService = termsService;
        _termValidatorsAggregate = termValidatorsAggregate;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TermCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTerm([FromForm] CreateTermRequest request)
    {
        await _termValidatorsAggregate.CreateTermValidator.ValidateAndThrowAsync(request);
        var result = await _termsService.CreateTerm(request);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpPost("delete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteTerms([FromBody] DeleteTermsRequest request)
    {
        await _termValidatorsAggregate.DeleteTermsValidator.ValidateAndThrowAsync(request);
        await _termsService.DeleteTerms(request);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TermUpdatedResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTerm([FromRoute] Guid id, [FromForm] UpdateTermRequest request)
    {
        await _termValidatorsAggregate.UpdateTermValidator.ValidateAndThrowAsync(request);
        var result = await _termsService.UpdateTerm(id, request);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagingResponse<TermResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTerms([FromQuery] TermPagingRequest request)
    {
        var result = await _termsService.GetTerms(request);
        return Ok(result);
    }

    [HttpDelete("{termId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTermImage([FromRoute] Guid termId)
    {
        await _termsService.DeleteTermImage(termId);
        return NoContent();
    }

}