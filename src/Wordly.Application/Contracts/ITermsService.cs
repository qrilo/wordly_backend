using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wordly.Application.Models.Common;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Contracts;

public interface ITermsService
{
    Task<IReadOnlyCollection<TermResponse>> GetTerms();
    Task<TermCreatedResponse> CreateTerm(CreateTermRequest request);
    Task DeleteTerms(DeleteTermsRequest request);
    Task<TermUpdatedResponse> UpdateTerm(Guid userTermId, UpdateTermRequest request);
    Task<PagingResponse<TermResponse>> GetTerms(PagingRequest request);
    Task DeleteTermImage(Guid termId);
}