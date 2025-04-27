using FitPlanner.Communication.Requests;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories;
using FitPlanner.Domain.Security.Cryptography;
using FitPlanner.Domain.Services.LoggedUser;

namespace FitPlanner.Application.UseCases.User.ChangePassword;

public class ChangeUserPasswordUseCase : IChangeUserPasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangeUserPasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository repository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangeUserPasswordJson request)
    {
        var loggedUser = await _loggedUser.User();

        Validate(request, loggedUser);
    }

    private void Validate(RequestChangeUserPasswordJson request, Domain.Entities.User loggedUser)
    {
        var result = new ChangeUserPasswordValidator().Validate(request);
        
        if (_passwordEncripter.Is(request.Password))
    }
}