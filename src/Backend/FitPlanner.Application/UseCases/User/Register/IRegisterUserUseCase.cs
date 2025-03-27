using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;

namespace FitPlanner.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request);
}