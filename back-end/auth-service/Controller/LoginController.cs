using Microsoft.AspNetCore.Mvc;
using Auth0.AuthenticationApi.Models;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authentication;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class LoginController : Controller
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    private readonly IConfiguration config;

    public LoginController(IConfiguration config)
    {
        this.config = config;
    }

    [HttpPost("api/auth/login")]
    public async Task<ActionResult> Login([FromBody] LoginModel request)
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
            Response.Cookies.Append("token",result.AccessToken, new CookieOptions{
                Path="/",
                HttpOnly = true,
                Secure=false
            });
            return Ok(new { access_token = result.AccessToken });
        }
        else
        {
            return Unauthorized();
        }
    }
    
    [Authorize]
    [HttpPost("api/auth/logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok();
    }

    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
}