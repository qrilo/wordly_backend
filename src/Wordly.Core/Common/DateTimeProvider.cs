using System;
using Wordly.Core.Contracts;

namespace Wordly.Core.Common;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}