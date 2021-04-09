using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Poc.LogTail.Core.DataAccess.Mongo.Documents
{
    [BsonIgnoreExtraElements]
    public class Product : Document<ObjectId>
    {
        [BsonElement("productId")] public string ProductId { get; set; }
        [BsonElement("productName")] public string ProductName { get; set; }
        [BsonElement("price")] public decimal Price { get; set; }
        [BsonElement("createdDateTimeUtc")] public DateTime CreatedDateTimeUtc { get; set; } = DateTime.UtcNow;
        [BsonElement("publishStatus")] public bool PublishStatus { get; set; }
        [BsonElement("resumeToken")] public string ResumeToken { get; set; }
    }
}
