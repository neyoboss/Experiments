using MongoDB.Driver;

public class MatchService : IMatchService
{
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<MatchModel> collection;
    private IMongoCollection<MatchedModels> matchedModelsCollection;

    public MatchService()
    {
        this.database = dbClient.GetDatabase("MatchTender");
        this.collection = database.GetCollection<MatchModel>("Match");
        this.matchedModelsCollection = database.GetCollection<MatchedModels>("Matched");
    }

    public async Task<bool> DeleteMatches(string CurrentProfileId)
    {
        return (await collection.DeleteOneAsync(currentProfile => currentProfile.CurrentProfileId == CurrentProfileId)).DeletedCount > 0;
    }

    public async Task<List<MatchModel>> GetProfileMatches(string CurrentProfileId)
    {
        return await collection.Find(profile => CurrentProfileId == profile.CurrentProfileId).ToListAsync();
    }

    public async Task<MatchedModels> CreateMatch(string model1, string model2)
    {
        var firstModel = await collection.Find(profile => model1 == profile.CurrentProfileId).FirstOrDefaultAsync();
        var secondProfile = await collection.Find(profile => model2 == profile.CurrentProfileId).FirstOrDefaultAsync();

        if (firstModel.CurrentProfileId == secondProfile.OtherProfileId || secondProfile.CurrentProfileId == firstModel.OtherProfileId)
        {
            var matchedModel = new MatchedModels
            {
                Profile1 = firstModel.CurrentProfileId,
                Profile2 = secondProfile.CurrentProfileId
            };

            await matchedModelsCollection.InsertOneAsync(matchedModel);

            return matchedModel;
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public async Task<string> MatchProfiles(MatchModel model)
    {
        if (model.isMatch == true)
        {
            await collection.InsertOneAsync(model);
            return "Profile added";
        }

        var firstProfile = await collection.Find(profile => model.CurrentProfileId == profile.CurrentProfileId && model.OtherProfileId == profile.OtherProfileId).FirstOrDefaultAsync();
        var secondProfile = await collection.Find(profile => model.CurrentProfileId == profile.OtherProfileId && model.OtherProfileId == profile.CurrentProfileId).FirstOrDefaultAsync();

        if (firstProfile != null && secondProfile != null)
        {
            var matchedModel = new MatchedModels
            {
                Profile1 = firstProfile.CurrentProfileId,
                Profile2 = secondProfile.CurrentProfileId
            };

            await matchedModelsCollection.InsertOneAsync(matchedModel);

            return "Created Match";
        }

        return "k";
    }
}