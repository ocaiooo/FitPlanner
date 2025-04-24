using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.Update;
using FitPlanner.Exceptions;
using FitPlanner.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        
        var act = await Record.ExceptionAsync(() => 
            useCase.Execute(request));
        
        Assert.Null(act);
        Assert.Equal(request.Name, user.Name);
        Assert.Equal(request.Email, user.Email);
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(user);
    
        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(
            async () => await useCase.Execute(request));
    
        Assert.Single(exception.ErrorMessages);
        Assert.Contains(ResourceMessagesException.NAME_EMPTY, exception.ErrorMessages);
        Assert.NotEqual(request.Name, user.Name);
        Assert.NotEqual(request.Email, user.Email);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var (user, _) = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);
    
        var exception = await Assert.ThrowsAsync<ErrorOnValidationException>(
            async () => await useCase.Execute(request));
    
        Assert.Single(exception.ErrorMessages);
        Assert.Contains(ResourceMessagesException.EMAIL_ALREADY_REGISTERED, exception.ErrorMessages);
        Assert.NotEqual(request.Name, user.Name);
        Assert.NotEqual(request.Email, user.Email);
    }

    private static UpdateUserUseCase CreateUseCase(FitPlanner.Domain.Entities.User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        if (!string.IsNullOrEmpty(email))
            userReadOnlyRepository.ExistActiveUserWithEmail(email);
        
        return new UpdateUserUseCase(loggedUser, userUpdateRepository, userReadOnlyRepository.Build(), unitOfWork);
    }
}