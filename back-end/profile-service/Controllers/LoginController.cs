using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    private readonly IConfiguration config;
    public LoginController(IConfiguration config)
    {
        this.config = config;
    }

    
    [HttpPost("api/profile/login")]
    public async Task<IActionResult> Login([FromBody] ProfileModel request)
    {
        // Use the Auth0 Authentication API to authenticate the user's credentials

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

    [Authorize]
    [HttpGet("api/getUser")]
    public async Task<IActionResult> Profile()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        return Ok(new UserAuth()
        {
            name = User.Identity.Name,
            email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
        });
    }

    public class UserAuth
    {
        public string? name { get; set; }
        public string? email { get; set; }
    }
}