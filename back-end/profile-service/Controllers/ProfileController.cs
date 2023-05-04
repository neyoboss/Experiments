using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ProfileController : ControllerBase
{
    IProfileService profileService;
    IRabbitMQProducer rabbitMQProducer;

    public ProfileController(IProfileService profileService, IRabbitMQProducer rabbitMQProducer)
    {
        this.profileService = profileService;
        this.rabbitMQProducer = rabbitMQProducer;
    }

    [HttpGet("api/profile/getAllProfiles")]
    public async Task<ActionResult<List<ProfileModel>>> GetAllProfiles()
    {
        try
        {
            return Ok(await profileService.GetProfiles());
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error getting profiles, {ex.Message}");
        }
    }

    [HttpGet("api/profile/{id}")]
    public async Task<ActionResult<ProfileModel>> GetProfile(Guid id)
    {
        try
        {
            rabbitMQProducer.SendMessage("Profile get by id");
            return Ok(await profileService.GetProfileById(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error getting profile, {ex.Message}");
        }
    }

    [HttpGet("api/profile/sendMessage")]
    public ActionResult<string> SendMessage()
    {
        try
        {
            rabbitMQProducer.SendMessage("Hello");
            return "Message send";
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    [HttpPut("api/profile/updateProfile")]
    public async Task<ActionResult<ProfileModel>> UpdateProfile(ProfileModel profile)
    {
        try
        {
            return Ok(await profileService.UpdateProfile(profile));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating profile");
        }
    }

    [HttpGet("api/profile/getProfilesWithoutCurrent/{currentProfileId}")]
    public async Task<ActionResult<List<ProfileModel>>> GetProfilesWithoutCurrent(string currentProfileId)
    {
        try
        {
            return Ok(await profileService.GetProfileModelsWithoutCurrentId(currentProfileId));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error fetching profiles: {e.Message}");
        }
    }

    [HttpDelete("api/profile/deleteProfile/{id}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        try
        {
            await profileService.DeleteProfile(id);
            rabbitMQProducer.SendMessage(id.ToString());
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error deleting profile: {e.Message}");
        }
    }

    [Authorize]
    [HttpGet("api/testService")]
    public async Task<ActionResult> Test()
    {
        return Ok("Tested auth");
    }
}