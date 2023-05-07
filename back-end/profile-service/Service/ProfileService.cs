using MongoDB.Driver;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Auth0.AuthenticationApi;

public class ProfileService : IProfileService
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri("https://dev-0ck6l5pnflrq01jd.eu.auth0.com"));
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<ProfileModel> collection;

    BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tenderblob;AccountKey=h+mQabKquq6HhmW/CVKKnUG1l5iUjeiTEHys06y4wXqiyltbQz/Pph3hxHmGJRaxDYZ4rPeaVP/i+ASti3NO0A==;EndpointSuffix=core.windows.net");

    public ProfileService()
    {
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<ProfileModel>("Profile");
    }

    #region AddImageToAzure
    public async void AddImagesToAzureBlolb(string profileId, byte[] imageBytes)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profileId);
        string imageName = Guid.NewGuid().ToString();
        BlobClient blobClient = containerClient.GetBlobClient(imageName);
        await blobClient.UploadAsync(new MemoryStream(imageBytes), new BlobHttpHeaders { ContentType = "image/jpeg" });
    }
    #endregion
    

    #region DeleteProfile
    public async Task<bool> DeleteProfile(string id)
    {
        return (await collection.DeleteOneAsync(profile => profile.id == id)).DeletedCount > 0;
    }
    #endregion
    
    public async Task<ProfileModel> GetProfileById(string id)
    {
        var profile = await collection.Find(profile => profile.id == id).FirstOrDefaultAsync();
        // BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profile.id.ToString());
        
        // List<string> imageUrls = new List<string>();
        // await foreach (var item in containerClient.GetBlobsAsync())
        // {
        //     BlobClient blobClient = containerClient.GetBlobClient(item.Name);
        //     imageUrls.Add(blobClient.Uri.AbsoluteUri);
        // }
        // profile.images = imageUrls;

        return profile;
    }

    public async Task<List<ProfileModel>> GetProfiles()
    {
        return await collection.Find(_ => true).ToListAsync();
    }

    public async Task<ProfileModel> UpdateProfile(ProfileModel updatedProfileModel)
    {
        await collection.ReplaceOneAsync(profile => profile.id == updatedProfileModel.id, updatedProfileModel);
        return updatedProfileModel;
    }

    public async Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string currnetProfileId)
    {
        return await collection.Find(profile => currnetProfileId != profile.id).ToListAsync();
    }
}