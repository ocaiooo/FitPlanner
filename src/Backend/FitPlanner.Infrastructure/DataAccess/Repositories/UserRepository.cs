using FitPlanner.Domain.Entities;
using FitPlanner.Domain.Respositories.User;

namespace FitPlanner.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserReadOnlyRepository,  IUserWriteOnlyRepository
{
    private readonly FitPlannerDbContext _dbContext;
    
    public UserRepository(FitPlannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<bool> ExistActiveUserWithEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task Add(User user)
    {
        throw new NotImplementedException();
    }
}