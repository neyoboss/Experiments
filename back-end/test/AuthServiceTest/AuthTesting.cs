using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace AuthServiceTest;

public class AuthTesting
{
    private readonly HttpClient _httpClient;

    public AuthTesting()
    {
        // Set up the HttpClient for making test requests
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7280"); // Update the base URL as per your API
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var loginModel = new LoginModel
        {
            email = "testuser@gmail.com",
            password = "asd123ASD!"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseStream = await response.Content.ReadAsStreamAsync();
        using var jsonDocument = await JsonDocument.ParseAsync(responseStream);
        var responseData = jsonDocument.RootElement;

        // Assert
        Assert.True(responseData.TryGetProperty("access_token", out var accessToken));
        Assert.NotNull(accessToken.GetString());

        Assert.True(responseData.TryGetProperty("user", out var user));
        // Additional assertions on the user object if necessary
    }
}