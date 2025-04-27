using FitPlanner.Domain.Security.Cryptography;
using FitPlanner.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Sha512Encripter("abc1234");
}