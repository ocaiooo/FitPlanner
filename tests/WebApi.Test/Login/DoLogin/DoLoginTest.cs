using System.Globalization;
using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FitPlanner.Communication.Requests;
using FitPlanner.Exceptions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : FitPlannerClassFixture
{
    private const string Method = "login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _email = factory.GetEmail();
        _password = factory.GetPassword();
        _name = factory.GetName();
    } 


    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };
        
        var response = await DoPost(Method, request);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var userName = responseData.RootElement.GetProperty("name").GetString();
        Assert.NotNull(userName);
        Assert.NotEmpty(userName);
        Assert.Equal(_name, userName);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Invalid(string culture)
    {
        var request = RequestLoginJsonBuilder.Build();
        
        var response = await DoPost(Method, request, culture);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));
        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }
}