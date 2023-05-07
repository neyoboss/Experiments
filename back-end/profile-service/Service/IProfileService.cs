public interface IProfileService
{
    Task<List<ProfileModel>> GetProfiles();
    Task<bool> DeleteProfile(string id);
    Task<ProfileModel> UpdateProfile(ProfileModel profileModel);
    Task<ProfileModel> GetProfileById(string id);
    Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string currnetProfileId);
}