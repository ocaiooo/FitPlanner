namespace FitPlanner.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    public string Generate();
}