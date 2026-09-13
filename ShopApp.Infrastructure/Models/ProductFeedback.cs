using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopInfrastructure.Models;

public class ProductFeedback
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("productId")]
    public int ProductId { get; set; }

    [BsonElement("type")]
    public string Type { get; set; } = string.Empty;

    [BsonElement("message")]
    public string Message { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }
}