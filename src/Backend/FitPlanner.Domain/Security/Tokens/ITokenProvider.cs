﻿namespace FitPlanner.Domain.Security.Tokens;

public interface ITokenProvider
{
    public string Value();
}