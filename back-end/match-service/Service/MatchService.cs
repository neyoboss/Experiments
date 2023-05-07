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

    public async Task CreateUpdateMatch(string id, User user)
    {
        var filter = Builders<MatchModel>.Filter.Eq("id", id);
        var matchModel = await collection.Find(m => m.id == id).FirstOrDefaultAsync();

        if (matchModel != null)
        {
            if (matchModel.MatchesForUser.TryGetValue(id, out List<User>? matches))
            {
                matches.Add(user);
            }

            var update = Builders<MatchModel>.Update.Set("MatchesForUser", matchModel.MatchesForUser);
            await collection.UpdateOneAsync(filter, update);
        }
        else
        {
            Console.WriteLine("kur nema");
        }
    }
}