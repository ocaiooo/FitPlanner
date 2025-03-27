namespace FitPlanner.Domain.Respositories;

public interface IUnitOfWork
{
    public Task Commit();
}