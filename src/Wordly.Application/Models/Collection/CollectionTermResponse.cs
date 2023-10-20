using System;

namespace Wordly.Application.Models.Collection;

public sealed record CollectionTermResponse
{
  public Guid Id { get; set; }
  public string Term { get; set; }
  public string Definition { get; set; }
  public string ImageUrl { get; set; }
}