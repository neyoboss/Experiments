using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Runtime.Caching;


[ApiController]
public class ProfileController : ControllerBase
{
    IProfileService profileService;
    IRabbitMQProducer rabbitMQProducer;

    private static readonly MemoryCache cache = MemoryCache.Default;

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
    public async Task<ActionResult<List<string>>> GetProfile(string id)
    {
        try
        {
            if (cache.Contains(id))
            {
                return Ok(cache.Get(id));
            }

            // rabbitMQProducer.SendMessage("Profile get by id");
            var images = await profileService.GetImagesForProfile(id);
            cache.Add(id, images, DateTimeOffset.UtcNow.AddMinutes(10));
            return Ok(images);
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

    [HttpGet("api/profile/getProfilesWithoutCurrent")]
    public async Task<ActionResult<List<ProfileModel>>> GetProfilesWithoutCurrent([FromHeader] string userId)
    {
        try
        {
            return Ok(await profileService.GetProfileModelsWithoutCurrentId(userId));
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error fetching profiles: {e.Message}");
        }
    }

    [HttpDelete("api/profile/deleteProfile/{id}")]
    public async Task<ActionResult> DeleteComment(string id)
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

    [HttpPost("api/profile/uploadImage")]
    public async Task<ActionResult<string>> UploadImage([FromForm] string userId, [FromForm] IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded");
            }
            string imageUrl = await profileService.AddImagesToAzureBlolb(userId, image);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("api/profile/deleteImage")]
    public async Task<ActionResult<string>> DeleteImage(string userId, string imageName)
    {
        try
        {
            await profileService.DeleteImageFromAzure(userId, imageName);
            return Ok("Image Deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("api/profile/createMatch")]
    public async Task<ActionResult> CreateMatch([FromBody] MatchCreationModel matchCreationModel)
    {
        string targetService = "https://localhost:7080/matches/update";
        string payloadJson = JsonConvert.SerializeObject(matchCreationModel);

        // Create an instance of HttpClient
        using (HttpClient httpClient = new HttpClient())
        {
            // Create a StringContent object with the JSON payload
            StringContent payload = new StringContent(payloadJson, encoding: System.Text.Encoding.UTF8, "application/json");

            // Send the POST request to the target service
            HttpResponseMessage response = await httpClient.PostAsync(targetService, payload);

            // Handle the response from the target service as needed
            if (response.IsSuccessStatusCode)
            {
                // The request was successful
                return Ok();
            }
            else
            {
                // The request failed
                return StatusCode((int)response.StatusCode);
            }
        }
    }
    public class MatchCreationModel
    {
        public ProfileModel LoggedUser { get; set; }
        public ProfileModel User { get; set; }
    }
}

