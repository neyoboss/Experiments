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
    public async Task<string> AddImagesToAzureBlolb(string profileId, IFormFile image)
    {
        profileId = profileId.Replace("|", "-");
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profileId);        
        BlobClient blobClient = containerClient.GetBlobClient($"{image.Name}");
        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = image.ContentType } });
        }
        return blobClient.Uri.ToString();
    }
    #endregion


    #region DeleteProfile
    public async Task<bool> DeleteProfile(string id)
    {
        return (await collection.DeleteOneAsync(profile => profile.id == id)).DeletedCount > 0;
    }
    #endregion



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

    public async Task<List<string>> GetImagesForProfile(string id)
    {
        //var profile = await collection.Find(profile => profile.id == id).FirstOrDefaultAsync();
        id = id.Replace("|", "-");
        // BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profile.id.ToString());

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(id);


        List<string> imageUrls = new List<string>();
        await foreach (var item in containerClient.GetBlobsAsync())
        {
            BlobClient blobClient = containerClient.GetBlobClient(item.Name);
            imageUrls.Add(blobClient.Uri.AbsoluteUri);
        }

        return imageUrls;
    }
}