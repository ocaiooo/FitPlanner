namespace FitPlanner.Domain.Repositories.RefreshToken;

public interface IRefreshTokenWriteOnlyRepository
{
    Task Add(Entities.RefreshToken refreshToken);
    Task DeleteByUserId(long userId);
}