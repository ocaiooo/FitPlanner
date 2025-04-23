using FitPlanner.Domain.Entities;
using FitPlanner.Domain.Repositories.User;
using FitPlanner.Domain.Respositories.User;
using Microsoft.EntityFrameworkCore;

namespace FitPlanner.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository,  IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly FitPlannerDbContext _dbContext;
    
    public UserRepository(FitPlannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetById(long id) => await _dbContext.Users.FirstAsync(user => user.Id == id);
    
    public void Update(User user) => _dbContext.Users.Update(user);
    
    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.Users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);
    
    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email.Equals(email) && user.Password.Equals(password));
    }
}