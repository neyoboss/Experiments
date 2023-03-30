using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ProfileModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid id { get; set; }
    public string? email { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
    public string? password { get; set; }
    public List<string>? images { get; set; }
}