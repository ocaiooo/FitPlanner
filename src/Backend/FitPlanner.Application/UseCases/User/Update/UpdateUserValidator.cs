﻿using FitPlanner.Communication.Requests;
using FitPlanner.Exceptions;
using FluentValidation;

namespace FitPlanner.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);

        When(request => !string.IsNullOrEmpty(request.Email), () =>
            {
                RuleFor(request => request.Email).EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
    }
}