using System.Net;
using System.Text.Json;
using CommonTestUtilities.Tokens;

namespace WebApi.Test.User.Profile;

public class GetUserProfileTest : FitPlannerClassFixture
{
    private const string Method = "user";

    private readonly string _name;
    private readonly string _email;
    private readonly Guid _userIdentifier;
    
    public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _name = factory.GetName();
        _email = factory.GetEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoGet(Method, token: token);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        var name = responseData.RootElement.GetProperty("name").GetString();
        var email = responseData.RootElement.GetProperty("email").GetString();
        
        Assert.NotNull(name);
        Assert.NotEmpty(name);
        Assert.Equal(_name, name);
        
        Assert.NotNull(email);
        Assert.NotEmpty(email);
        Assert.Equal(_email, email);
    }
}