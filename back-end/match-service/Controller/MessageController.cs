using Microsoft.AspNetCore.Mvc;
using System.Net.Http;


[ApiController]
public class MessageController : ControllerBase
{
    IMessageReceive messageReceive;
    IMatchService matchService;
    public MessageController(IMessageReceive messageReceive, IMatchService matchService)
    {
        this.messageReceive = messageReceive;
        this.matchService = matchService;
    }

    [HttpGet("api/matches/getAllProfiles")]
    public async Task<ActionResult> GetAllProfiles()
    {
        HttpClient httpClient = new HttpClient();
        try
        {
            //change the addres when deploying
            return Ok(await httpClient.GetFromJsonAsync<Object>("https://localhost:7282/api/getAllProfiles"));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }

    [HttpGet("api/matches/profileMatches/{currentProfileId}")]
    public async Task<ActionResult> GetProfileMatches(string currentProfileId)
    {
        try
        {
            return Ok(await matchService.GetProfileMatches(currentProfileId));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }


    [HttpPost("api/matches/match")]
    public async Task<ActionResult> Match(MatchModel model)
    {
        try
        {
            return Ok(await matchService.MatchProfiles(model));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
        }
    }
}