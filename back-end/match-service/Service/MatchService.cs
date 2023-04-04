using MongoDB.Driver;

public class MatchService : IMatchService
{
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;

    public MatchService()
    {
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<MatchModel>("Match");
    }

    public Task<bool> DeleteMatches(string CurrentProfileId)
    {
        throw new NotImplementedException();
    }

    public void MatchProfiles()
    {
        throw new NotImplementedException();
    }
}