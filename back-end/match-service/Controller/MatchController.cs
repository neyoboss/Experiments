using Microsoft.AspNetCore.Mvc;

[ApiController]
public class MatchController : ControllerBase
{
    IMatchService matchService;
    public MatchController(IMatchService matchService)
    {
        this.matchService = matchService;
    }

    [HttpPost("match/update")]
    public async Task<ActionResult> UpdateMatch([FromBody] User user)
    {
        
        User send = new User{
            id = user.id,
            email = user.email,
            firstName = user.firstName,
            lastName = user.lastName
        };
        try
        {
            await matchService.CreateUpdateMatch("123-asd", send);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return Ok("User updated");
    }
}