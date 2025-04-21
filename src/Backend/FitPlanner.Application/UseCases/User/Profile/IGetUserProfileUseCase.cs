using FitPlanner.Communication.Responses;

namespace FitPlanner.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}