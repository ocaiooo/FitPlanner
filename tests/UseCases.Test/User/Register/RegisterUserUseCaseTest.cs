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
    
    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        
        return new RegisterUserUseCase(userReadOnlyRepository,userWriteOnlyRepository, unitOfWork, mapper, passwordEncripter);
    }
}