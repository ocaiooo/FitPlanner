namespace FitPlanner.Domain.Respositories.User;

public interface IUserReadOnlyRepository
{
    public Task<bool> ExistActiveUserWithEmail(string email);
    
    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);

    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier);
}