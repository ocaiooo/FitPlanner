using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using FitPlanner.Application.UseCases.User.Profile;

namespace UseCases.Test.User.Profile;

public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);
        
        var result = await useCase.Execute();
        
        Assert.NotNull(result);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(FitPlanner.Domain.Entities.User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser, mapper);
    }
}