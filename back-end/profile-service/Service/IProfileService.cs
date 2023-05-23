public interface IProfileService
{
    Task<List<ProfileModel>> GetProfiles();
    Task<bool> DeleteProfile(string id);
    Task<ProfileModel> UpdateProfile(ProfileModel profileModel);
    Task<ImagesModel> GetImagesForProfile(string id);
    Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string id);
    Task<string> AddImagesToAzureBlolb(string profileId, IFormFile image);
    Task<string> DeleteImageFromAzure(string profileId, string imageName);
}