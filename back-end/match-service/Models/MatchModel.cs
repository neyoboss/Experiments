using MongoDB.Bson.Serialization.Attributes;

public class MatchModel
{
    [BsonExtraElements]
    public Dictionary<string, List<bool>>? CurrentProfile { get; set; }
    public Dictionary<string, bool>? OtherProfileMatch { get; set; }
}