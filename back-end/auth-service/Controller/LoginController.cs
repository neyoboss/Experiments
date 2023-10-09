using Microsoft.AspNetCore.Mvc;
using Auth0.AuthenticationApi.Models;
using Auth0.AuthenticationApi;
using Microsoft.AspNetCore.Authorization;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using MongoDB.Driver;

[ApiController]
public partial class LoginController : ControllerBase
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri(""));
    MongoClient dbClient = new MongoClient("");
    private readonly IConfiguration config;
    private IMongoDatabase database;
    private IMongoCollection<LoginModel> collection;
    IRabbitMqProducer rabbitMqProducer;

    public LoginController(IConfiguration config, IRabbitMqProducer rabbitMqProducer)
    {
        this.rabbitMqProducer = rabbitMqProducer;
        this.config = config;
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<LoginModel>("Profile");
    }

    [AllowAnonymous]
    [HttpPost("api/auth/register")]
    public async Task<ActionResult> Register([FromBody] LoginModel model)
    {
        try
        {
            var auth0ManageClient = new ManagementApiClient(config["Auth0:ApiToken"], config["Auth0:Domain"]);

            var auth0UserReq = new UserCreateRequest
            {
                Email = model.email,
                Password = model.password,
                FirstName =  model.firstName,
                LastName = model.lastName,
                Connection = "Tender"
            };

            var createUser = await auth0ManageClient.Users.CreateAsync(auth0UserReq);

            var user = new LoginModel
            {
                id = createUser.UserId,
                // id = "123-asd",
                email = model.email,
                firstName = model.firstName,
                lastName = model.lastName
            };

            rabbitMqProducer.SendMessage(user, "blob", "AzureBlobCreation");

            await collection.InsertOneAsync(user);
            return Ok("user created");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
            Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });
            var user = await _auth0Client.GetUserInfoAsync(result.AccessToken);

            return Ok(new { access_token = result.AccessToken, user });
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpGet("api/auth/getHealth")]
    public ActionResult GetHealth(){
        Response.Headers.Add("Cache-Control","public, max-age=3600");
        return Ok("Hallooooo");
    }
    
    [HttpPost("api/auth/logout")]
    public ActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok("Logged out");
    }
}
