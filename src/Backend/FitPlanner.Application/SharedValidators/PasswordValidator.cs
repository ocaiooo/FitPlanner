﻿using FitPlanner.Exceptions;
using FluentValidation;
using FluentValidation.Validators;

namespace FitPlanner.Application.SharedValidators;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_TOO_SHORT);

            return false;
        }

        if (password.Length < 6)
        {
            context.MessageFormatter.AppendArgument("ErrorMessage", ResourceMessagesException.PASSWORD_TOO_SHORT);

            return false;
        }

        return true;
    }

    public override string Name => "PasswordValidator";
    
    protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}";
}