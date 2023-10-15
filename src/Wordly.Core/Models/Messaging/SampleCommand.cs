﻿using System;

namespace Wordly.Core.Models.Messaging;

public sealed record SampleCommand
{
    public Guid Id { get; init; }
    public int UsersCount { get; init; }
}