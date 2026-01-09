using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;
using FitPlanner.Domain.Repositories;
using FitPlanner.Domain.Repositories.RefreshToken;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Security.Cryptography;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Exceptions.ExceptionsBase;

namespace FitPlanner.Application.UseCases.Login.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    private const int RefreshTokenExpirationDays = 7;

    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenWriteOnlyRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        var encriptedPassword = _passwordEncripter.Encrypt(request.Password);

        var user = await _repository.GetByEmailAndPassword(request.Email, encriptedPassword) ?? throw new InvalidLoginException();

        var refreshToken = new Domain.Entities.RefreshToken
        {
            Token = _refreshTokenGenerator.Generate(),
            UserId = user.Id,
            ExpiresOn = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays)
        };

        await _refreshTokenRepository.DeleteByUserId(user.Id);
        await _refreshTokenRepository.Add(refreshToken);
        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
                RefreshToken = refreshToken.Token
            }
        };
    }
}