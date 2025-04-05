using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FitPlanner.Exceptions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi.Test.InlineData;

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

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
        
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
        
        var response = await _httpClient.PostAsJsonAsync("User", request);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        
        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        Assert.Single(errors);
        Assert.Contains(errors, error => error.GetString()!.Equals(expectedMessage));
    }
}