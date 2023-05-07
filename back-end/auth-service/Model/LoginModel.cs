using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
public class LoginModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string? id {get;set;}
    public string? email { get; set; }
    public string? password { get; set; }
    public string? firstName { get; set; }
    public string? lastName { get; set; }
}