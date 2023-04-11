using MongoDB.Driver;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

public class ProfileService : IProfileService
{
    MongoClient dbClient = new MongoClient("mongodb+srv://neykneyk1:081100neyko@tender.55ndihf.mongodb.net/test");
    private IMongoDatabase database;
    private IMongoCollection<ProfileModel> collection;

    BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=tenderblob;AccountKey=h+mQabKquq6HhmW/CVKKnUG1l5iUjeiTEHys06y4wXqiyltbQz/Pph3hxHmGJRaxDYZ4rPeaVP/i+ASti3NO0A==;EndpointSuffix=core.windows.net");

    public ProfileService()
    {
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<ProfileModel>("Profile");
    }

    public async Task<List<string>> GetImagesFromAzureBlobOnId(string profileId)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profileId);

        List<string> imageUrls = new List<string>();
        await foreach (var item in containerClient.GetBlobsAsync())
        {
            BlobClient blobClient = containerClient.GetBlobClient(item.Name);
            imageUrls.Add(blobClient.Uri.AbsoluteUri);
        }
        return imageUrls;
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
    
    #region RegisterProfile
    public async Task<ProfileModel> RegisterProfile(ProfileModelDTO profileDTO)
    {
        var profileModel = new ProfileModel
        {
            email = profileDTO.email,
            firstName = profileDTO.firstName,
            lastName = profileDTO.lastName,
            password = profileDTO.password,
        };
        await collection.InsertOneAsync(profileModel);
        return profileModel;
    }
    #endregion

    #region DeleteProfile
    public async Task<bool> DeleteProfile(Guid id)
    {
        return (await collection.DeleteOneAsync(profile => profile.id == id)).DeletedCount > 0;
    }
    #endregion
    
    public async Task<ProfileModel> GetProfileById(Guid id)
    {
        var profile = await collection.Find(profile => profile.id == id).FirstOrDefaultAsync();
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profile.id.ToString());
        
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

        // BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(updatedProfileModel.id.ToString());
        // if (!await containerClient.ExistsAsync())
        // {
        //     await containerClient.CreateAsync(PublicAccessType.BlobContainer);
        // }

        await collection.ReplaceOneAsync(profile => profile.id == updatedProfileModel.id, updatedProfileModel);
        return updatedProfileModel;
    }

    public async Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string currnetProfileId)
    {
        return await collection.Find(profile => new Guid(currnetProfileId) != profile.id).ToListAsync();
    }
}