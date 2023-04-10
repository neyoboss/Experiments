using MongoDB.Driver;

public class MatchService : IMatchService
{
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;

    public MatchService()
    {
        this.database = dbClient.GetDatabase("MatchTender");
        this.collection = database.GetCollection<MatchModel>("Match");
    }

    public async Task<bool> DeleteMatches(string CurrentProfileId)
    {
        return (await collection.DeleteOneAsync(currentProfile => currentProfile.CurrentProfileId == CurrentProfileId)).DeletedCount > 0;
    }

    public async Task<List<MatchModel>> GetProfileMatches(string CurrentProfileId)
    {
        return await collection.Find(profile => CurrentProfileId == profile.CurrentProfileId).ToListAsync();
    }

    public async Task<string> MatchProfiles(MatchModel model)
    {
        if (model.isMatch == true)
        {
            await collection.InsertOneAsync(model);
        }
        return "Profile added";
    }
}