using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.Update;
using FitPlanner.Exceptions;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);
        
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new UpdateUserValidator();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new UpdateUserValidator();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, result.Errors[0].ErrorMessage);
    }


    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();
        
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "invalid-email";
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_INVALID, result.Errors[0].ErrorMessage);
    }
}