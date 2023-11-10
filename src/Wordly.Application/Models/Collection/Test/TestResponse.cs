namespace Wordly.Application.Models.Collection.Test;

public sealed record TestResponse
{
     public QuestionType QuestionType { get; set; }
     public MatchResponse Match { get; set; }
     public SingleResponse Single { get; set; }
     public WrittenResponse Written { get; set; }
}