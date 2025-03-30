using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.Register;
using FitPlanner.Exceptions;

namespace Validators.Test.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();

        var request = RequestRegisterUserJsonBuilder.Build();
        
        var result = validator.Validate(request);

        Assert.True(result.IsValid); 
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new RegisterUserValidator();
        
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, result.Errors[0].ErrorMessage);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new RegisterUserValidator();
        
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_EMPTY, result.Errors[0].ErrorMessage);
    }
  
    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "invalid-email";
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.EMAIL_INVALID, result.Errors[0].ErrorMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLenght)
    {
        var validator = new RegisterUserValidator();
        
        var request = RequestRegisterUserJsonBuilder.Build(passwordLenght);
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PASSWORD_TOO_SHORT, result.Errors[0].ErrorMessage);
    }
}