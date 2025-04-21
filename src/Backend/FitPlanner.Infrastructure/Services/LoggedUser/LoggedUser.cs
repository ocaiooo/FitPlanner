using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FitPlanner.Domain.Entities;
using FitPlanner.Domain.Security.Tokens;
using FitPlanner.Domain.Services.LoggedUser;
using FitPlanner.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly FitPlannerDbContext _context;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(FitPlannerDbContext context, ITokenProvider tokenProvider)
    {
        _context = context;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> User()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
        
        var userIdentifier = Guid.Parse(identifier);
        
        return await _context
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.Active && user.UserIdentifier == userIdentifier);
    }
}