using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FitPlanner.Application.UseCases.User.Register;
using FitPlanner.Exceptions;
using FitPlanner.Exceptions.ExceptionsBase;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{ 
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase();
        
        var result = await useCase.Execute(request);
        
        Assert.NotNull(result);
        Assert.NotNull(result.Tokens);
        Assert.NotNull(result.Tokens.AccessToken);
        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception =  await Assert.ThrowsAsync<ErrorOnValidationException>(() => act());
        
        Assert.Single(exception.ErrorMessages);
        Assert.Equal(ResourceMessagesException.EMAIL_ALREADY_REGISTERED, exception.ErrorMessages.First());
    }
    
    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exception =  await Assert.ThrowsAsync<ErrorOnValidationException>(() => act());
        
        Assert.Single(exception.ErrorMessages);
        Assert.Equal(ResourceMessagesException.NAME_EMPTY, exception.ErrorMessages.First());
    }
    
    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();


        if (string.IsNullOrEmpty(email) == false)
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(email);
        
        return new RegisterUserUseCase(userReadOnlyRepositoryBuilder.Build(),userWriteOnlyRepository, unitOfWork, mapper, passwordEncripter, accessTokenGenerator);
    }
}