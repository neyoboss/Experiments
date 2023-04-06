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

    [HttpGet("api/getAllProfiles")]
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

    [HttpGet("api/images/{id}")]
    public async Task<ActionResult<List<string>>> GetImages(string id)
    {
        try
        {
            return Ok(await profileService.GetImagesFromAzureBlobOnId(id));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error getting profile, {ex.Message}");
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

    [HttpGet("api/sendMessage")]
    public ActionResult<string> SendMessage()
    {
        rabbitMQProducer.SendMessage("Hello");
        return "Message send";
    }

    [HttpPost("api/registerProfile")]
    public async Task<ActionResult<ProfileModel>> RegisterProfile(ProfileModelDTO profile)
    {
        try
        {
            var addProfile = await profileService.RegisterProfile(profile);
            //rabbitMQProducer.SendMessage(profile);

            return Ok(addProfile);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error registering profile: {e.Message}");
        }

    }

    [HttpPut("api/updateProfile")]
    public async Task<ActionResult<ProfileModel>> UpdateProfile(ProfileModel profile)
    {
        try
        {
            return Ok(await profileService.UpdateProfile(profile));
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating comment");
        }
    }
}