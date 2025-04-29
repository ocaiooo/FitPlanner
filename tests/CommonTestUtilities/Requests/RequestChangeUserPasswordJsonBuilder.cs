using Bogus;
using FitPlanner.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangeUserPasswordJsonBuilder
{
    public static RequestChangeUserPasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestChangeUserPasswordJson>()
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.NewPassword, f => f.Internet.Password(passwordLength));
    }
}