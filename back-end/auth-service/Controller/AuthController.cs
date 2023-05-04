using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AuthController:Controller{
    [Route("api/new/login")]
     public async Task Login(string returnUrl = "google.com")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri("http://localhost:3000")
                .WithScope("email openid profile")
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        [Route("api/new/logout")]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in 
                .WithRedirectUri("http://localhost:3000")
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        [Route("api/new/profile")]
        public ActionResult<string> Profile()
        {
            var token = HttpContext.GetTokenAsync(Auth0Constants.AuthenticationScheme, "access_token");
            return $@"{User.Identity.Name}
            {User.Claims.FirstOrDefault(c=> c.Type == "picture")?.Value}
            {User.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.Email)?.Value}
            { User.Claims.FirstOrDefault(c => c.Type == "AuthenticationToken")?.Value}
            ";
        }

        [Route("api/new/test")]
        public ActionResult Test()
        {
            using(var client = new HttpClient()){
               var res = client.GetAsync("https://localhost:7282/api/testService");
                return Ok(res);
            }
        }
}