using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.ChangePassword;
using FitPlanner.Exceptions;

namespace Validators.Test.User.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangeUserPasswordValidator();

        var request = RequestChangeUserPasswordJsonBuilder.Build();
        
        var result = validator.Validate(request);

        Assert.True(result.IsValid); 
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Error_Password_Invalid(int passwordLenght)
    {
        var validator = new ChangeUserPasswordValidator();
        
        var request = RequestChangeUserPasswordJsonBuilder.Build(passwordLenght);
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PASSWORD_TOO_SHORT, result.Errors[0].ErrorMessage);
    }
    
    [Fact]
    public void Error_Password_Empty()
    {
        var validator = new ChangeUserPasswordValidator();
        
        var request = RequestChangeUserPasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        
        var result = validator.Validate(request);
        
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(ResourceMessagesException.PASSWORD_EMPTY, result.Errors[0].ErrorMessage);
    }
}