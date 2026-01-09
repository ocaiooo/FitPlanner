using FitPlanner.Domain.Repositories.RefreshToken;
using Moq;

namespace CommonTestUtilities.Repositories;

public class RefreshTokenWriteOnlyRepositoryBuilder
{
    public static IRefreshTokenWriteOnlyRepository Build()
    {
        var mock = new Mock<IRefreshTokenWriteOnlyRepository>();

        return mock.Object;
    }
}