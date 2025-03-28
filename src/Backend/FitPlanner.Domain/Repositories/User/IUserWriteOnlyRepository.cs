namespace FitPlanner.Domain.Respositories.User;

public interface IUserWriteOnlyRepository
{
    public Task Add(Entities.User user);
}