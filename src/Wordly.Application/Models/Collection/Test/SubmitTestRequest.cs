using System.Collections.Generic;

namespace Wordly.Application.Models.Collection.Test;

public sealed record SubmitTestRequest
{
    public IReadOnlyCollection<AnswerModel> Answers { get; init; }
}