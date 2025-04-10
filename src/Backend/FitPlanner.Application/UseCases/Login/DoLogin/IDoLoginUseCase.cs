using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;

namespace FitPlanner.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}