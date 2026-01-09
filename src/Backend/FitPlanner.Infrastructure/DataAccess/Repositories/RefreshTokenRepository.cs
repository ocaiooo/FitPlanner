using FitPlanner.Domain.Entities;
using FitPlanner.Domain.Repositories.RefreshToken;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure.DataAccess.Repositories;

public class RefreshTokenRepository : IRefreshTokenReadOnlyRepository, IRefreshTokenWriteOnlyRepository
{
    private readonly FitPlannerDbContext _dbContext;

    public RefreshTokenRepository(FitPlannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(RefreshToken refreshToken) => await _dbContext.RefreshTokens.AddAsync(refreshToken);

    public async Task DeleteByUserId(long userId)
    {
        var tokens = await _dbContext.RefreshTokens.Where(rt => rt.UserId == userId).ToListAsync();
        _dbContext.RefreshTokens.RemoveRange(tokens);
    }

    public async Task<RefreshToken?> GetByToken(string token)
    {
        return await _dbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token && rt.Active);
    }
}