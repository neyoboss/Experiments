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

    public async Task<bool> DeleteMatches(string profileId)
    {

        // await matchedModelsCollection.DeleteOneAsync(profile => profile.Profile1 == profileId || profile.Profile2 == profileId);
        // await collection.DeleteOneAsync(profile => profile.CurrentProfileId == profileId || profile.OtherProfileId == profileId);
        return (await collection.DeleteOneAsync(profile => profile.OtherProfileId == profileId)).DeletedCount > 0;
    }

    public async Task<List<MatchModel>> GetProfileMatches(string CurrentProfileId)
    {
        return await collection.Find(profile => CurrentProfileId == profile.CurrentProfileId).ToListAsync();
    }

    public async Task<string> MatchProfiles(MatchModel model)
    {
        if (model.isMatch == true)
        {
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
                await collection.DeleteOneAsync(profile => model.CurrentProfileId == profile.CurrentProfileId && model.OtherProfileId == profile.OtherProfileId);
                await collection.DeleteOneAsync(profile => model.CurrentProfileId == profile.OtherProfileId && model.OtherProfileId == profile.CurrentProfileId);
                return "Created Match";
            }
            else
            {
                await collection.InsertOneAsync(model);
                return "Profile added";
            }
        }
        return "k";
    }
}