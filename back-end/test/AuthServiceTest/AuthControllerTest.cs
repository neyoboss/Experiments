using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthServiceTest;

public class AuthControllerTest : IDisposable
{
    private readonly HttpClient httpClient;
    public AuthControllerTest()
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7280");
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public void Dispose()
    {
        httpClient.Dispose();
    }

    [Fact]
    public async Task Login_Ok()
    {
        //Arange
        var loginModel = new LoginModel
        {
            email = "testuser@gmail.com",
            password = "asd123ASD!"
        };

        //Act
        var response = await httpClient.PostAsJsonAsync("api/auth/login", loginModel);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var responseStream = await response.Content.ReadAsStreamAsync();
        using var jsonDocument = await JsonDocument.ParseAsync(responseStream);
        var responseData = jsonDocument.RootElement;

        // Assert
        Assert.True(responseData.TryGetProperty("access_token", out var accessToken));
        Assert.NotNull(accessToken.GetString());

        Assert.True(responseData.TryGetProperty("user", out var user));
    }
}