﻿namespace Wordly.Core.Contracts;

public interface IHashingProvider
{
    public string GetHash(string value);
    public bool Verify(string value, string hash);
}