using MongoDB.Bson.Serialization.Attributes;
public class MatchedModels
{
    [BsonId]
    public Guid id{get;set;}
    public string? Profile1 {get;set;}
    public string? Profile2 {get;set;}
}