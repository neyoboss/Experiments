public interface IMatchService
{
    Task<List<MatchModel>> GetProfileMatches(string CurrentProfileId);
    Task<string> MatchProfiles(MatchModel model);
    Task<bool> DeleteMatches(string CurrentProfileId);
}