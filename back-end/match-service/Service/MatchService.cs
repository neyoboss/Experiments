using MongoDB.Bson;
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

    public async Task CreateUpdateMatch(string Id, User loggedUser, User user)
    {
        var filter = Builders<MatchModel>.Filter.Eq(m => m.Id, Id);
        var update = Builders<MatchModel>.Update.Push("MatchesForUser",new KeyValuePair<User, List<User>>(loggedUser,new List<User>{user}));
        await collection.UpdateOneAsync(filter,update);
    }
}