public interface IMatchService
{
    void MatchProfiles();
    Task<bool> DeleteMatches(string CurrentProfileId);
}