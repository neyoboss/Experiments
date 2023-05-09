using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class MatchModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string? id { get; set; }

    public List<User> MatchesForUser;
}