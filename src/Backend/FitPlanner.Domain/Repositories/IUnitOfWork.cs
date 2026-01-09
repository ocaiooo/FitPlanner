namespace FitPlanner.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}