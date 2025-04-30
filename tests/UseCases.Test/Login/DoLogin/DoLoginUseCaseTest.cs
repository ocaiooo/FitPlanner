using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FitPlanner.Application.UseCases.Login.DoLogin;
using FitPlanner.Communication.Requests;
using FitPlanner.Exceptions;
using FitPlanner.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        });
        
        Assert.NotNull(result);
        Assert.NotNull(result.Name);
        Assert.NotNull(result.Tokens);
        Assert.NotNull(result.Tokens.AccessToken);
        Assert.Equal(user.Name, result.Name);
    }

    [Fact]
    public async Task Error_Invalid_User()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();
        
        Func<Task> act = async () => await useCase.Execute(request);

        var exception =  await Assert.ThrowsAsync<InvalidLoginException>(() => act());
        
        Assert.Equal(ResourceMessagesException.EMAIL_OR_PASSWORD_INVALID, exception.Message);
    }

    private static DoLoginUseCase CreateUseCase(FitPlanner.Domain.Entities.User? user = null)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();
        

        if (user is not null)
            userReadOnlyRepositoryBuilder.GetByEmailAndPassword(user);
        
        return new DoLoginUseCase(userReadOnlyRepositoryBuilder.Build(), passwordEncripter, accessTokenGenerator);
    }
}