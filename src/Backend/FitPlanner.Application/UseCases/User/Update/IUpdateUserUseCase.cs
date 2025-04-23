using FitPlanner.Communication.Requests;

namespace FitPlanner.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(RequestUpdateUserJson request);
}