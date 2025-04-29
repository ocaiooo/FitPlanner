using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;

namespace UseCases.Test.User.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    private async Task Success()
    {
        var (user, password) = UserBuilder.Build();

        var request = RequestChangeUserPasswordJsonBuilder.Build();
        request.Password = password;
        // Todo: createUseCase method
        var useCase = CreateUseCase(user);
        
        Func<Task> act = async () => await useCase.Execute(request);

        
    }
}