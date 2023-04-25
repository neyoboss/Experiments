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
    // [HttpGet("api/login")]
    // public async Task<string> Login()
    // {
    //     try
    //     {
    //         using var client = new HttpClient();
    //         client.BaseAddress = new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com");

    //         var response = await client.PostAsync("oauth/token", new FormUrlEncodedContent(
    //             new Dictionary<string, string>
    //             {
    //                     { "grant_type", "client_credentials" },
    //                     { "client_id", "s6lB4cA0A2Cz9pz65tgkpCLt2fjhYgs1" },
    //                     { "client_secret", "jgPf8_H4vrOUf7-C6PsQBuyUuB3yP7fEWV8K9SE3bgLl4Sbn0MqsNXPhA3OnqhOu" },
    //                     { "audience", "https://tender-auth/" }
    //             }
    //         ));

    //         var content = await response.Content.ReadAsStringAsync();
    //         var jsonResult = JObject.Parse(content);
    //         var mgmtToken = jsonResult["access_token"]?.Value<string>() ?? "";
    //         return mgmtToken;
    //     }
    //     catch (Exception ex)
    //     {
    //         return ex.Message;
    //     }
    // }
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    private readonly IConfiguration config;
    public LoginController(IConfiguration config)
    {
        this.config = config;
    }

    // [HttpPost("api/login")]
    // public async Task<ObjectResult> Login(string emial, string password)
    // {
    //     var authenticationProperties = new LoginAuthenticationPropertiesBuilder().WithRedirectUri("https://localhost:7280/callback").WithAudience("https://tender-auth/").Build();
    //     await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
    //     return Ok(HttpContext.GetTokenAsync("access_token"));
    // }

    [HttpPost("api/login")]
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
                //ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            });
        }

        public class UserAuth{
            public string? name {get;set;}
            public string? email {get;set;}
        }
    }