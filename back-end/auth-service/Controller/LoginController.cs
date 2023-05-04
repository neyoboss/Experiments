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
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private readonly IConfiguration config;
    private IMongoDatabase database;
    private IMongoCollection<Object> collection;
    IRabbitMqProducer rabbitMqProducer;

    public LoginController(IConfiguration config, IRabbitMqProducer rabbitMqProducer)
    {
        this.rabbitMqProducer = rabbitMqProducer;
        this.config = config;
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<Object>("Profile");
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
                Connection = "Tender"
            };

            var createUser = await auth0ManageClient.Users.CreateAsync(auth0UserReq);

            var user = new User
            {
                UserId = createUser.UserId,
                Email = model.email,
                FirstName = model.firstName,
                LastName = model.lastName
            };

            rabbitMqProducer.SendMessage(user);
            
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
            Response.Cookies.Append("token", result.AccessToken, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = false
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
}