public interface IProfileService
{
    Task<ProfileModel> RegisterProfile(ProfileModelDTO profile);
    Task<List<ProfileModel>> GetProfiles();
    Task<bool> DeleteProfile(Guid id);
    Task<ProfileModel> UpdateProfile(ProfileModel profileModel);
    Task<ProfileModel> GetProfileById(Guid id);
    Task<List<string>> GetImagesFromAzureBlobOnId(string profileId);
}