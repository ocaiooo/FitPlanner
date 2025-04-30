using System.Net;
using CommonTestUtilities.Tokens;

namespace WebApi.Test.User.Profile;

public class GetUserProfileInvalidTokenTest : FitPlannerClassFixture
{
    private const string Method = "user";

    public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var response = await DoGet(Method, token: "tokenInvalid");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var response = await DoGet(Method, token: string.Empty);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoGet(Method, token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}