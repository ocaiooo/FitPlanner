using FitPlanner.Communication.Requests;

namespace FitPlanner.Application.UseCases.User.ChangePassword;

public interface IChangeUserPasswordUseCase
{
    public Task Execute(RequestChangeUserPasswordJson request);
}