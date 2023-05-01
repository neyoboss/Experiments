using Microsoft.AspNetCore.Mvc;

[ApiController]
public class MessageController : ControllerBase
{
    //  matchService;
    // public MessageController(IMatchService matchService)
    // {
 
    //     this.matchService = matchService;
    // }

    // [HttpGet("api/matches/profileMatches/{currentProfileId}")]
    // public async Task<ActionResult> GetProfileMatches(string currentProfileId)
    // {
    //     try
    //     {
    //         return Ok(await matchService.GetProfileMatches(currentProfileId));
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
    //     }
    // }

    // [HttpPost("api/matches/match")]
    // public async Task<ActionResult> Match(MatchModel model)
    // {
    //     try
    //     {IMatchService
    //         return Ok(await matchService.MatchProfiles(model));
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
    //     }
    // }
    
    // [HttpDelete("api/matches/deleteMatch/{id}")]
    // public async Task<ActionResult> Match(string id)
    // {
    //     try
    //     {
    //         return Ok(await matchService.DeleteMatches(id));
    //     }
    //     catch (Exception e)
    //     {
    //         return StatusCode(StatusCodes.Status500InternalServerError, $"{e.Message}");
    //     }
    // }
}