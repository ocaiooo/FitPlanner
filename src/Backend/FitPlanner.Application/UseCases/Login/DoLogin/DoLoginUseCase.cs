using FitPlanner.Application.Services.Cryptography;
using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories;
using FitPlanner.Domain.Respositories.User;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Exceptions.ExceptionsBase;

namespace FitPlanner.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly PasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public DoLoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }
    
    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);
        
        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
        };
    }
}