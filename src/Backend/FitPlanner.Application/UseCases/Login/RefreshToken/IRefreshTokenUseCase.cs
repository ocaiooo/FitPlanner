using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;

namespace FitPlanner.Application.UseCases.Login.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<ResponseTokensJson> Execute(RequestRefreshTokenJson request);
}