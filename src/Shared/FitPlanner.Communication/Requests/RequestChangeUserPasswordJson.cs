﻿namespace FitPlanner.Communication.Requests;

public class RequestChangeUserPasswordJson
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}