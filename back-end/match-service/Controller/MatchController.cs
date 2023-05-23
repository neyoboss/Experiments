using Microsoft.AspNetCore.Mvc;

[ApiController]
public class MatchController : ControllerBase
{
    IMatchService matchService;
    public MatchController(IMatchService matchService)
    {
        this.matchService = matchService;
    }

    [HttpPost("matches/update")]
    public async Task<ActionResult> UpdateMatch()
    {


        try
        {
            User loggedUser = new User
            {
                id = "auth0|64640c7bc70de2f8085b93fd",
                email = "neykoTest@gmail.com",
                firstName = "Neyko",
                lastName = "Neykov"
            };

            User user = new User
            {
                id = "auth0|646cff402edf0713b92ea83e",
                email = "asd@gmail.com",
                firstName = "Jef",
                lastName = "Ma"
            };

            await matchService.CreateUpdateMatch(loggedUser, user);

            return Ok("User updated");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    public class UserBody
    {
        public User loggedUser { get; set; }
        public User user { get; set; }
    }
}