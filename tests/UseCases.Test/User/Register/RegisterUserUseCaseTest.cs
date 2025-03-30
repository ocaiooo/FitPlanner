using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.Register;

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
        Assert.Equal(request.Name, result.Name);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var useCase = CreateUseCase(request.Email);
    }
    
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrEmpty(email) == false)
            userReadOnlyRepositoryBuilder.ExistActiveUseWithEmail(email);
        
        return new RegisterUserUseCase(userReadOnlyRepositoryBuilder.Build(),userWriteOnlyRepository, unitOfWork, mapper, passwordEncripter);
    }
}