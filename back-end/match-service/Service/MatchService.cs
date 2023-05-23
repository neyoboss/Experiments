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
}