using Microsoft.AspNetCore.Mvc;
using Auth0.AuthenticationApi.Models;
using Auth0.AuthenticationApi;

public class LoginController : Controller
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    private readonly IConfiguration config;

    public LoginController(IConfiguration config)
    {
        this.config = config;
    }

    [HttpPost("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginModel request)
    {
        var result = await _auth0Client.GetTokenAsync(new ResourceOwnerTokenRequest
        {
            Audience = config["Auth0:Audience"],
            Scope = "openid",
            ClientId = config["Auth0:ClientId"],
            ClientSecret = config["Auth0:ClientSecret"],
            Username = request.email,
            Password = request.password
        });

        if (result != null)
        {
            return Ok(new { access_token = result.AccessToken });
        }
        else
        {
            return Unauthorized();
        }
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}