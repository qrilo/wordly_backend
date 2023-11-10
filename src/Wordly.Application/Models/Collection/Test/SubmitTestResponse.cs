using System.Collections.Generic;
using Wordly.Application.Models.Terms;

namespace Wordly.Application.Models.Collection.Test;

public sealed record SubmitTestResponse
{
    public int TotalQuantity { get; init; }
    public int TotalKnown { get; init; }
    public int TotalUnknown { get; init; }
    public double CorrectAnswersPercent { get; init; }
    public IReadOnlyCollection<TermResponse> WrongAnswerTerms { get; init; }
}