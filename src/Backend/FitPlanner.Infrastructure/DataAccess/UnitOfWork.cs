using FitPlanner.Domain.Respositories;

namespace FitPlanner.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly FitPlannerDbContext _dbContext;
    
    public UnitOfWork(FitPlannerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Commit()
    { 
        await _dbContext.SaveChangesAsync();
    }
}