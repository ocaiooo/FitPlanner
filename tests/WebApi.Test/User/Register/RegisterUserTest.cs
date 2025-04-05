using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public RegisterUserTest(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();
    
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        
        var response = await _httpClient.PostAsJsonAsync("User", request);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var userName = responseData.RootElement.GetProperty("name").GetString();
        Assert.NotNull(userName);
        Assert.NotEmpty(userName);
        Assert.Equal(request.Name, userName);
    }
}