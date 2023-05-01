public interface IProfileService
{
    Task<List<ProfileModel>> GetProfiles();
    Task<bool> DeleteProfile(Guid id);
    Task<ProfileModel> UpdateProfile(ProfileModel profileModel);
    Task<ProfileModel> GetProfileById(Guid id);
    Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string currnetProfileId);
}