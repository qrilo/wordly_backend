using System;

namespace Wordly.Application.Models.Collection.Test;

public sealed record AnswerModel
{
    public Guid Id { get; init; }
    public string Answer { get; init; }
}