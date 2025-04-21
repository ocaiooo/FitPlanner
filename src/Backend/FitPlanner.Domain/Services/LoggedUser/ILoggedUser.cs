using FitPlanner.Domain.Entities;

namespace FitPlanner.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    public Task<User> User();
}