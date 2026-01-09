using FitPlanner.Domain.Repositories;
using FitPlanner.Domain.Repositories.RefreshToken;
using FitPlanner.Domain.Services.LoggedUser;

namespace FitPlanner.Application.UseCases.Login.Logout;

public class LogoutUseCase : ILogoutUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUseCase(
        ILoggedUser loggedUser,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        var user = await _loggedUser.User();

        await _refreshTokenRepository.DeleteByUserId(user.Id);
        await _unitOfWork.Commit();
    }
}