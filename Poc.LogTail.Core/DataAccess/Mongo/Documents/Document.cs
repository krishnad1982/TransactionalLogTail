using MongoDB.Bson.Serialization.Attributes;

namespace Poc.LogTail.Core.DataAccess.Mongo.Documents
{
    public class Document<TKey>
    {
        [BsonId]
        public TKey Id { get; set; }
    }
}
