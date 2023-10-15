using System;

namespace Wordly.Core.Contracts;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}