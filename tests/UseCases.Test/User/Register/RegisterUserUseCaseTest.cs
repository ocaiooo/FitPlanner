using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Requests;
using FitPlanner.Application.UseCases.User.Register;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var mapper = MapperBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        
        var useCase = new RegisterUserUseCase(passwordEncripter, mapper);

        var result = await useCase.Execute(request);
        
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
    }
}