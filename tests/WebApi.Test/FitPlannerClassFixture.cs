using System.Net.Http.Json;

namespace WebApi.Test;

public class FitPlannerClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public FitPlannerClassFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "")
    {
        ChangeRequestCulture(culture);
        
        return await _httpClient.PostAsJsonAsync(method, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        if (_httpClient.DefaultRequestHeaders.Contains("Accept-Language"))
            _httpClient.DefaultRequestHeaders.Remove("Accept-Language");
        
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}