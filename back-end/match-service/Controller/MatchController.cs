using Microsoft.AspNetCore.Mvc;

[ApiController]
public class MatchController : ControllerBase
{
    IMatchService matchService;
    public MatchController(IMatchService matchService)
    {
        this.matchService = matchService;
    }

    // [HttpPost("matches/update")]
    // public async Task<ActionResult> UpdateMatch()
    // {


    //     try
    //     {
    //         User loggedUser = new User
    //         {
    //             id = "auth0|64640c7bc70de2f8085b93fd",
    //             email = "neykoTest@gmail.com",
    //             firstName = "Neyko",
    //             lastName = "Neykov"
    //         };

    //         User user = new User
    //         {
    //             id = "auth0|646cff402edf0713b92ea83e",
    //             email = "asd@gmail.com",
    //             firstName = "Jef",
    //             lastName = "Ma"
    //         };

    //         await matchService.CreateUpdateMatch(loggedUser, user);

    //         return Ok("User updated");
    //     }
    //     catch (Exception ex)
    //     {
    //         return BadRequest(ex.Message);
    //     }

    // }

    [HttpPost("matches/update")]
    public async Task<ActionResult> UpdateMatch([FromBody] MatchCreationModel matchCreationModel)
    {


        try
        {
            //     User loggedUser = new User
            //     {
            //         id = "auth0|64640c7bc70de2f8085b93fd",
            //         email = "neykoTest@gmail.com",
            //         firstName = "Neyko",
            //         lastName = "Neykov"
            //     };

            //     User user = new User
            //     {
            //         id = "auth0|646cff402edf0713b92ea83e",
            //         email = "asd@gmail.com",
            //         firstName = "Jef",
            //         lastName = "Ma"
            //     };
                User loggedUser = matchCreationModel.loggedUser;
                User user = matchCreationModel.user;
                await matchService.CreateUpdateMatch(loggedUser,user);
            // Console.WriteLine($"LoggedUser: {matchCreationModel.loggedUser.firstName}");
            // Console.WriteLine($"User: {matchCreationModel.user.firstName}");
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("matches/getIds/{id}")]
    public async Task<ActionResult<List<string>>> GetIds(string id)
    {
        return Ok(await matchService.ids(id));
    }

    public class MatchCreationModel
    {
        public User loggedUser { get; set; }
        public User user { get; set; }
    }
}