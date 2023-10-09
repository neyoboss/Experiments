using MongoDB.Bson;
using MongoDB.Driver;

public class MatchService : IMatchService
{
    MongoClient dbClient = new MongoClient("");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;

    public MatchService()
    {
        this.database = dbClient.GetDatabase("MatchTender");
        this.collection = database.GetCollection<MatchModel>("Match");
    }

    public async Task CreateUpdateMatch(User loggedUser, User user)
    {
        var loggedFilter = Builders<MatchModel>.Filter.Eq("id", loggedUser.id);
        var loggedMatchModel = await collection.Find(m => m.id == loggedUser.id).FirstOrDefaultAsync();


        var otherFilter = Builders<MatchModel>.Filter.Eq("id", user.id);
        var otherMatchModel = await collection.Find(m => m.id == user.id).FirstOrDefaultAsync();


        if (loggedMatchModel != null)
        {
            loggedMatchModel.MatchesForUser.Add(user);

            otherMatchModel.MatchesForUser.Add(loggedUser);

            var update = Builders<MatchModel>.Update.Set("MatchesForUser", loggedMatchModel.MatchesForUser);
            await collection.UpdateOneAsync(loggedFilter, update);

            var otheUpdate=Builders<MatchModel>.Update.Set("MatchesForUser", otherMatchModel.MatchesForUser);
            await collection.UpdateOneAsync(otherFilter, otheUpdate);

        }
        else
        {
            Console.WriteLine("kur nema");
        }
    }

    public async Task<List<string>> ids(string id)
    {
        List<string> userIds = new List<string>();
        var profile = await collection.Find(u => u.id == id).FirstOrDefaultAsync();
        foreach(User user in profile.MatchesForUser){
            userIds.Add(user.id);
        }
        return userIds;
    }
}
