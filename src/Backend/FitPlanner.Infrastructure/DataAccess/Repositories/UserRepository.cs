using FitPlanner.Domain.Entities;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories.User;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository,  IUserWriteOnlyRepository
{
    private readonly FitPlannerDbContext _dbContext;
    
    public UserRepository(FitPlannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password));
    }
}