using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class MatchModel
{
    [BsonId]
    public string? CurrentProfileId { get; set; }
    public string? OtherProfileId {get;set;}   
    public bool? isMatch {get;set;}
}