using MongoDB.Driver;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Auth0.AuthenticationApi;

public class ProfileService : IProfileService
{
    private readonly AuthenticationApiClient _auth0Client = new AuthenticationApiClient(new Uri(""));
    MongoClient dbClient = new MongoClient("");
    private IMongoDatabase database;
    private IMongoCollection<ProfileModel> collection;

    BlobServiceClient blobServiceClient = new BlobServiceClient("");

    public ProfileService()
    {
        this.database = dbClient.GetDatabase("Tender");
        this.collection = database.GetCollection<ProfileModel>("Profile");
    }

    #region AddImageToAzure
    public async Task<string> AddImagesToAzureBlolb(string profileId, IFormFile image)
    {
        profileId = profileId.Replace("|", "-");
        string imageName = $"{image.FileName}-{Guid.NewGuid()}";
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profileId);
        BlobClient blobClient = containerClient.GetBlobClient(imageName);
        using (var stream = image.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = image.ContentType } });
        }
        return imageName;
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

    public async Task<List<ProfileModel>> GetProfileModelsWithoutCurrentId(string id)
    {
        return await collection.Find(profile => id != profile.id).ToListAsync();
    }

    public async Task<ImagesModel> GetImagesForProfile(string id)
    {
        id = id.Replace("|", "-");

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(id);

        ImagesModel images = new ImagesModel();
        images.ImageName = new List<string>();
        images.ImageUrl = new List<string>();
        await foreach (var item in containerClient.GetBlobsAsync())
        {
            BlobClient blobClient = containerClient.GetBlobClient(item.Name);
            images.ImageUrl.Add(blobClient.Uri.AbsoluteUri);
            images.ImageName.Add(blobClient.Name);
        }

        return images;
    }

    public async Task<string> DeleteImageFromAzure(string profileId, string imageName)
    {
        profileId = profileId.Replace("|", "-");

        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(profileId);

        BlobClient blobClient = containerClient.GetBlobClient(imageName);

        await blobClient.DeleteAsync();

        return "Image Deleted";
    }
}
