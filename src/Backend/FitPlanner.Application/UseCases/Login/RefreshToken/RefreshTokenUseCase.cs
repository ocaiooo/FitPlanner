using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;
using FitPlanner.Domain.Repositories;
using FitPlanner.Domain.Repositories.RefreshToken;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Exceptions.ExceptionsBase;

namespace FitPlanner.Application.UseCases.Login.RefreshToken;

public class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly IRefreshTokenReadOnlyRepository _refreshTokenReadOnlyRepository;
    private readonly IRefreshTokenWriteOnlyRepository _refreshTokenWriteOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    private const int RefreshTokenExpirationDays = 7;

    public RefreshTokenUseCase(
        IRefreshTokenReadOnlyRepository refreshTokenReadOnlyRepository,
        IRefreshTokenWriteOnlyRepository refreshTokenWriteOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenReadOnlyRepository = refreshTokenReadOnlyRepository;
        _refreshTokenWriteOnlyRepository = refreshTokenWriteOnlyRepository;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseTokensJson> Execute(RequestRefreshTokenJson request)
    {
        var storedRefreshToken = await _refreshTokenReadOnlyRepository.GetByToken(request.RefreshToken);

        if (storedRefreshToken is null)
            throw new RefreshTokenNotFoundException();

        if (storedRefreshToken.ExpiresOn < DateTime.UtcNow)
            throw new RefreshTokenExpiredException();

        var user = storedRefreshToken.User;

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            Token = _refreshTokenGenerator.Generate(),
            UserId = user.Id,
            ExpiresOn = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays)
        };

        await _refreshTokenWriteOnlyRepository.DeleteByUserId(user.Id);
        await _refreshTokenWriteOnlyRepository.Add(newRefreshToken);
        await _unitOfWork.Commit();

        return new ResponseTokensJson
        {
            AccessToken = _accessTokenGenerator.Generate(user.UserIdentifier),
            RefreshToken = newRefreshToken.Token
        };
    }
}