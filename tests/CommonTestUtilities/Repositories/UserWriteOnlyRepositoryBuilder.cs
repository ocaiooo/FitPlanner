using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();
        
        return mock.Object;
    }
}