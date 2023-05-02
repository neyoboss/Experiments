using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class MatchModel
{
    public string Id { get; set; }

    [BsonExtraElements]
    public Dictionary<User,List<User>> MatchesForUser;
}